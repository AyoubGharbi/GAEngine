# Third-party notices

GAEngine's [MIT licence](LICENSE) covers the project's own code. Third-party
components retain their own copyright and licence notices.

## Vendored OpenTK / ImGui integration

These four files under `GAEngine/GAEngine/Gui/` come from
[Julius Häger (NogginBops)'s ImGui.NET_OpenTK_Sample](https://github.com/NogginBops/ImGui.NET_OpenTK_Sample):

- `ImGuiController.cs`
- `Shader.cs`
- `Texture.cs`
- `Util.cs`

Before the public-release cleanup, they matched the upstream
[2020 snapshot e2a1b492](https://github.com/NogginBops/ImGui.NET_OpenTK_Sample/tree/e2a1b492abc15105fde745a0ccf8c1c2f3a8a2a7/Dear%20ImGui%20Sample)
apart from the namespace and whitespace. This identifies a matching source
snapshot; it does not establish which commit was originally copied. The cleanup
adds attribution headers and corrects stale comments about the graphics backend.

The controller in that sample identifies itself as a modification of
[Veldrid.ImGui's ImGuiRenderer](https://github.com/veldrid/veldrid/tree/master/src/Veldrid.ImGui),
by Eric Mellino and Veldrid contributors. GAEngine renders the UI with OpenTK /
OpenGL; Veldrid is credited as source ancestry and is not a runtime dependency.

The upstream MIT notices are included verbatim:

- [ImGui.NET_OpenTK_Sample](licenses/ImGui.NET_OpenTK_Sample-MIT.txt)
  — copyright (c) 2025 Julius Häger, as stated in the upstream licence currently
  published by the project. The matching 2020 snapshot predates that licence file.
- [Veldrid](licenses/Veldrid-MIT.txt)
  — copyright (c) 2017 Eric Mellino and Veldrid contributors.

These notices are also copied into the Sandbox build output. The integration and
its helpers should not be presented as original GAEngine renderer/UI work.

## Package dependencies

NuGet restores these separately; their upstream licences apply. Consult the
licences accompanying the particular package versions when redistributing them.

| Component | Version in this project | Use / upstream |
| --- | --- | --- |
| OpenTK | 3.2.0 | Window, input, maths and OpenGL bindings — [OpenTK](https://github.com/opentk/opentk) |
| AssimpNet | 4.1.0 | Managed model import API and native Assimp binaries — [AssimpNet](https://www.nuget.org/packages/AssimpNet/4.1.0), [Assimp](https://github.com/assimp/assimp) |
| ImGui.NET | 1.75.0 | .NET bindings and native cimgui binaries — [ImGui.NET](https://github.com/ImGuiNET/ImGui.NET) |
| Dear ImGui / cimgui | Supplied through ImGui.NET | Immediate-mode UI and C wrapper — [Dear ImGui](https://github.com/ocornut/imgui), [cimgui](https://github.com/cimgui/cimgui) |
| NUnit | 3.12.0 | Historical unused reference; no tests are implemented — [NUnit](https://github.com/nunit/nunit) |

NuGet also resolves transitive .NET support libraries. This table describes the
main dependencies, not a complete inventory of all restored packages.

## Assets

`res/demo/cube.obj` and `res/demo/checker.png` were generated for this project in
2026 with [`tools/generate_demo_assets.py`](tools/generate_demo_assets.py). They
contain simple geometry and a checker pattern and are covered by the repository's
MIT licence. No third-party model or texture is required to run the demo.

`res/terrain.blend`, `res/terrain.fbx`, `res/color_hex_00.png` and the historical
screenshot predate this cleanup and are not used by the current demo. Their
original authorship is not recorded in the repository; the project owner should
confirm their provenance before distributing them under the project's licence.
