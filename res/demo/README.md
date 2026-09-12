# Demo assets

`cube.obj` and `checker.png` were created for GAEngine's public sample in 2026.
They are generated from simple geometry and a two-colour checker pattern, with no
downloaded models, images or external asset dependencies. Both are covered by the
repository's [MIT licence](../../LICENSE).

The cube has one mesh, 12 triangles, outward normals and UV coordinates. It is
loaded through Assimp just like an imported FBX model. The texture is a 128 × 128
RGB PNG. Both files are committed; Python is only needed to regenerate them:

```sh
python tools/generate_demo_assets.py
```

The build copies these files into `res/demo/` beside `Sandbox.exe`. **Refresh
texture** reloads that output copy of `checker.png`, so edit that copy to try a
texture change while the demo runs. Rebuilding can replace it with the source copy.
