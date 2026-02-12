# Depth-Guided Next Best View Planning for Efficient Close-Range Photogrammetry

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

## Overview

This repository contains the implementation of a novel approach to automated image capture for 3D reconstruction. Instead of using traditional volumetric representations (like 3D voxel grids), our method works directly with 2D depth images to determine where a camera should be positioned to capture high-quality 3D models of objects.

## The Problem

When creating 3D models from photographs (photogrammetry), a critical question is: **where should I position my camera to get the best results?** Taking too many photos wastes time and storage, while too few photos results in incomplete or inaccurate models. Traditional "Next-Best-View" (NBV) methods analyze the entire 3D space to find good camera positions, which requires significant memory and computation.

## Our Solution

We propose a method that:

1. **Analyzes a single depth image** to identify "Geometries of Interest" (GOIs)—surface regions with complex shapes that are prone to reconstruction errors
2. **Synthesizes new camera viewpoints** that specifically target these challenging regions with optimal viewing angles
3. **Reduces the number of required photos by 40-70%** compared to uniform sampling approaches while maintaining or improving accuracy

### Key Innovation

By working with 2D depth maps instead of 3D volumes, our approach has **O(W×H) complexity** (proportional to image width × height) rather than O(n³) for volumetric methods. This makes it suitable for high-resolution applications and resource-constrained environments.

## Three Acquisition Strategies Evaluated

The research evaluates three methods for capturing images:

- **Control**: Systematic uniform sampling around the object (baseline approach)
- **NBV**: Pure depth-guided next-best-view planning (our method)
- **Augmented**: Hybrid approach combining sparse uniform sampling with NBV viewpoints

Results show that the **Augmented approach** offers the best balance, combining comprehensive coverage with geometric accuracy, while the pure NBV method achieves superior accuracy with significantly fewer viewpoints.

## Repository Structure

```
.
├── Ipol/                                    # Implementation directory
│   ├── Depth-Guided-NBV-Planner.ipynb      # Main Jupyter notebook
│   ├── utils.py                             # Helper functions
│   ├── data/                                # Test data directory
│   │   ├── *.jpg                            # Color images
│   │   └── *.exr                            # Depth images (16-bit float)
│   └── README.md                            # Implementation instructions
├── LICENSE                                  # Apache 2.0 License
└── README.md                                # This file
```

## Method Pipeline

1. **Input**: RGB image + corresponding depth map from a reference viewpoint
2. **Surface Geometry Analysis**: Detect geometrically complex regions using scale-invariant depth map analysis
3. **Viewpoint Synthesis**: Generate optimal camera poses that converge on identified regions
4. **Image Capture**: Acquire images from synthesized viewpoints
5. **3D Reconstruction**: Process images using standard photogrammetry software
6. **Evaluation**: Compare reconstruction quality across acquisition strategies

## Getting Started

### Prerequisites

```bash
pip install numpy matplotlib opencv-python scikit-image scikit-learn scipy rembg open3d pyvista jupyter
```

**Note**: Ensure OpenEXR support for OpenCV:
```bash
export OPENCV_IO_ENABLE_OPENEXR=1
```

### Quick Start

1. Navigate to the `Ipol` directory:
   ```bash
   cd Ipol
   ```

2. Launch the Jupyter notebook:
   ```bash
   jupyter notebook Depth-Guided-NBV-Planner.ipynb
   ```

3. Follow the notebook workflow to process test images or your own data

For detailed instructions on replicating the full experimental pipeline, see [`Ipol/README.md`](Ipol/README.md).

## Applications

- **Industrial Quality Control**: Faster automated inspection with fewer images
- **Cultural Heritage Documentation**: Reduced handling time for fragile artifacts
- **Damage Assessment**: Natural focus on deformations and irregularities
- **Field Photogrammetry**: Reduced computational requirements for remote work

## Key Findings

- **Viewpoint Efficiency**: NBV method requires 40-70% fewer viewpoints than uniform sampling
- **Geometric Accuracy**: NBV achieves superior reconstruction accuracy in majority of test cases
- **Coverage Trade-off**: Pure NBV prioritizes geometric complexity over uniform coverage
- **Hybrid Performance**: Augmented approach balances coverage and accuracy, ranking highest overall
- **Computational Advantage**: O(W×H) complexity enables high-resolution applications

## Limitations

- The pure NBV method prioritizes geometric complexity over uniform coverage, which may result in incomplete reconstructions when used alone
- Working distance formulation may affect feature matching in certain scenarios
- The hybrid Augmented approach addresses these limitations by combining both strategies

## Citation

If you use this code in your research, please cite:

```bibtex
[Citation information will be added upon publication]
```

## License

Copyright 2026 [Author Name]

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

## Contact

[Contact information will be added upon publication]
