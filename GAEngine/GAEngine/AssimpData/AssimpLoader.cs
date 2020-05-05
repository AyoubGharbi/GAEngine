using Assimp;
using Assimp.Configs;
using GAEngine.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;


namespace GAEngine.AssimpData
{
    class AssimpLoader
    {
        private Scene m_model;
        private Vector3 m_sceneCenter, m_sceneMin, m_sceneMax;
        private float m_angle;
        private int m_displayList;
        private int m_texId;

        public Scene Model => m_model;
        public int TextureID => m_texId;

        public AssimpLoader()
        {
            String fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Res\terrain.fbx");

            AssimpContext importer = new AssimpContext();
            importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));

            m_model = importer.ImportFile(fileName, PostProcessPreset.TargetRealTimeMaximumQuality);

            ComputeBoundingBox();
        }

        private void ComputeBoundingBox()
        {
            m_sceneMin = new Vector3(1e10f, 1e10f, 1e10f);
            m_sceneMax = new Vector3(-1e10f, -1e10f, -1e10f);
            Matrix4 identity = Matrix4.Identity;

            ComputeBoundingBox(m_model.RootNode, ref m_sceneMin, ref m_sceneMax, ref identity);

            m_sceneCenter.X = (m_sceneMin.X + m_sceneMax.X) / 2.0f;
            m_sceneCenter.Y = (m_sceneMin.Y + m_sceneMax.Y) / 2.0f;
            m_sceneCenter.Z = (m_sceneMin.Z + m_sceneMax.Z) / 2.0f;
        }

        private void ComputeBoundingBox(Node node, ref Vector3 min, ref Vector3 max, ref Matrix4 trafo)
        {
            Matrix4 prev = trafo;
            trafo = Matrix4.Mult(prev, node.Transform.FromMatrix());

            if (node.HasMeshes)
            {
                foreach (int index in node.MeshIndices)
                {
                    Mesh mesh = m_model.Meshes[index];
                    for (int i = 0; i < mesh.VertexCount; i++)
                    {
                        Vector3 tmp = mesh.Vertices[i].FromVector();
                        Vector3.TransformVector(ref tmp, ref trafo, out tmp);

                        min.X = Math.Min(min.X, tmp.X);
                        min.Y = Math.Min(min.Y, tmp.Y);
                        min.Z = Math.Min(min.Z, tmp.Z);

                        max.X = Math.Max(max.X, tmp.X);
                        max.Y = Math.Max(max.Y, tmp.Y);
                        max.Z = Math.Max(max.Z, tmp.Z);
                    }
                }
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                ComputeBoundingBox(node.Children[i], ref min, ref max, ref trafo);
            }
            trafo = prev;
        }

        public void Unload()
        {
            GL.DeleteTexture(m_texId);
        }

        public void Resize(int width, int height)
        {
            float aspectRatio = width / (float)height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }

        public void Update(FrameEventArgs e)
        {
            m_angle += 25f * (float)e.Time;
            if (m_angle > 360)
            {
                m_angle = 0.0f;
            }
        }

        public void Render()
        {
            GL.ClearColor(Color.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Normalize);
            GL.FrontFace(FrontFaceDirection.Ccw);

            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.LoadMatrix(ref lookat);

            GL.Rotate(m_angle, 0.0f, 1.0f, 0.0f);

            float tmp = m_sceneMax.X - m_sceneMin.X;
            tmp = Math.Max(m_sceneMax.Y - m_sceneMin.Y, tmp);
            tmp = Math.Max(m_sceneMax.Z - m_sceneMin.Z, tmp);
            tmp = 1.0f / tmp;
            GL.Scale(tmp * 2, tmp * 2, tmp * 2);

            GL.Translate(-m_sceneCenter);

            if (m_displayList == 0)
            {
                m_displayList = GL.GenLists(1);
                GL.NewList(m_displayList, ListMode.Compile);
                RecursiveRender(m_model, m_model.RootNode);
                GL.EndList();
            }

            GL.CallList(m_displayList);

        }

        private void RecursiveRender(Scene scene, Node node)
        {
            Matrix4 m = node.Transform.FromMatrix();
            m.Transpose();
            GL.PushMatrix();
            GL.MultMatrix(ref m);

            if (node.HasMeshes)
            {
                foreach (int index in node.MeshIndices)
                {
                    Mesh mesh = scene.Meshes[index];
                    ApplyMaterial(scene.Materials[mesh.MaterialIndex]);

                    if (mesh.HasNormals)
                    {
                        GL.Enable(EnableCap.Lighting);
                    }
                    else
                    {
                        GL.Disable(EnableCap.Lighting);
                    }

                    bool hasColors = mesh.HasVertexColors(0);
                    if (hasColors)
                    {
                        GL.Enable(EnableCap.ColorMaterial);
                    }
                    else
                    {
                        GL.Disable(EnableCap.ColorMaterial);
                    }

                    bool hasTexCoords = mesh.HasTextureCoords(0);

                    foreach (Face face in mesh.Faces)
                    {
                        OpenTK.Graphics.OpenGL.PrimitiveType faceMode;
                        switch (face.IndexCount)
                        {
                            case 1:
                                faceMode = OpenTK.Graphics.OpenGL.PrimitiveType.Points;
                                break;
                            case 2:
                                faceMode = OpenTK.Graphics.OpenGL.PrimitiveType.Lines;
                                break;
                            case 3:
                                faceMode = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
                                break;
                            default:
                                faceMode = OpenTK.Graphics.OpenGL.PrimitiveType.Polygon;
                                break;
                        }

                        GL.Begin(faceMode);
                        for (int i = 0; i < face.IndexCount; i++)
                        {
                            int indice = face.Indices[i];
                            if (hasColors)
                            {
                                Color4 vertColor = mesh.VertexColorChannels[0][indice].FromColor();
                            }
                            if (mesh.HasNormals)
                            {
                                Vector3 normal = mesh.Normals[indice].FromVector();
                                GL.Normal3(normal);
                            }
                            if (hasTexCoords)
                            {
                                Vector3 uvw = mesh.TextureCoordinateChannels[0][indice].FromVector();
                                GL.TexCoord2(uvw.X, 1 - uvw.Y);
                            }
                            Vector3 pos = mesh.Vertices[indice].FromVector();
                            GL.Vertex3(pos);
                        }
                        GL.End();
                    }
                }
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                RecursiveRender(m_model, node.Children[i]);
            }
        }

        private void LoadTexture(String fileName)
        {
            fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Res/" + fileName);
            if (!File.Exists(fileName))
            {
                return;
            }
            Bitmap textureBitmap = new Bitmap(fileName);
            BitmapData TextureData =
                    textureBitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, textureBitmap.Width, textureBitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb
                );
            m_texId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, m_texId);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, textureBitmap.Width, textureBitmap.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, TextureData.Scan0);
            textureBitmap.UnlockBits(TextureData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        private void ApplyMaterial(Material mat)
        {
            if (mat.GetMaterialTextureCount(TextureType.Diffuse) > 0)
            {
                TextureSlot tex;
                if (mat.GetMaterialTexture(TextureType.Diffuse, 0, out tex))
                    LoadTexture(tex.FilePath);
            }

            Color4 color = new Color4(.8f, .8f, .8f, 1.0f);
            if (mat.HasColorDiffuse)
            {
                color = mat.ColorDiffuse.FromColor();
            }
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, color);

            color = new Color4(0, 0, 0, 1.0f);
            if (mat.HasColorSpecular)
            {
                color = mat.ColorSpecular.FromColor();
            }
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, color);

            color = new Color4(.2f, .2f, .2f, 1.0f);
            if (mat.HasColorAmbient)
            {
                color = mat.ColorAmbient.FromColor();
            }
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, color);

            color = new Color4(0, 0, 0, 1.0f);
            if (mat.HasColorEmissive)
            {
                color = mat.ColorEmissive.FromColor();
            }
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, color);

            float shininess = 1;
            float strength = 1;
            if (mat.HasShininess)
            {
                shininess = mat.Shininess;
            }
            if (mat.HasShininessStrength)
            {
                strength = mat.ShininessStrength;
            }

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, shininess * strength);
        }


    }
}
