# Depth-Guided Next-Best-View Planner

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

A depth-guided approach to Next-Best-View (NBV) planning using depth images and blob detection to generate optimal camera poses for object reconstruction.

## Overview

This project tests the hypothesis that range images are sufficient for proposing candidate views in NBV planning. The system:
- Detects geometries of interest (GoI) using depth-based blob detection
- Generates primary and secondary camera poses for each GoI

## Requirements

```bash
pip install numpy matplotlib opencv-python scikit-image scikit-learn scipy rembg[cpu] open3d pyvista jupyter pyvista[jupyter] onnxruntime
```

**Note**: Ensure OpenEXR support for OpenCV:
```bash
export OPENCV_IO_ENABLE_OPENEXR=1
```

## Project Structure

```
.
├── Depth-Guided-NBV-Planner.ipynb  # Main Jupyter notebook
├── utils.py                         # Helper functions
├── data/                            # Image pairs directory
│   ├── *.jpg                        # Color images
│   └── *.exr                        # Depth images (16-bit float)
├── LICENSE                          # Apache 2.0 License
└── README.md
```

## Usage

### Running the Notebook

```bash
jupyter notebook Depth-Guided-NBV-Planner.ipynb
```

The notebook processes image pairs from the `data/` folder. Five sample image pairs are included to demonstrate the workflow. Users can add their own color (`.jpg`) and depth (`.exr`) image pairs with matching filenames.

**Workflow:**
1. **Load Images** - Load color/depth pair from `data/` folder
2. **Create Mask** - Automatic foreground segmentation
3. **Detect Blobs** - Multi-scale blob detection on depth map
4. **Cluster GoIs** - Mean Shift clustering of detected features
5. **Generate Poses** - Compute primary/secondary camera poses
6. **Visualize** - Interactive 3D visualization with PyVista
7. **Export** - Look-at vectors for camera control

### Output

The final cell generates `lookat_vectors`:
```python
[[target_pos, primary_camera_pos, secondary_camera_pos], ...]
```

Each entry contains:
- **target_pos**: ROI centroid (look-at point)
- **primary_camera_pos**: Primary camera position
- **secondary_camera_pos**: Secondary camera position

These look-at vectors can be directly imported into the Unity application provided in the supplementary materials for camera control and view planning.

## Citation

If you use this code in your research, please cite:

```bibtex
@misc{chesang2026nbv,
  author = {Andrew Chesang},
  title = {Depth-Guided Next-Best-View Planner},
  year = {2026},
  publisher = {GitHub},
  url = {https://github.com/yourusername/Depth-Guided-NBV-Planner}
}
```

## License

Copyright 2026 Andrew Chesang

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
