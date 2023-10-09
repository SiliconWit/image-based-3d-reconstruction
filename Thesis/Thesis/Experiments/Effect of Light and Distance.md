
![[Pasted image 20230830232528.png]]I investigated 2 models, the Utah Teapot with patterned textures and a RandomObjects that I designed in Blender.
![[Pasted image 20230830232502.png]]
### RandowmShape2SpiralNoLamp125
Sfm -> 153511
No Lamp
[00:05:11] [Register] Final RMS*: 1.6334 (computed on 49873 points)
261020 Points

### UtahTeapotCircularLamp_70_0.5_1.5
Points = 52710
[22:58:16] [Register] Final RMS*: 2.59208 (computed on 49651 points)
[23:06:49] [Distribution fitting] Gauss: mean = 1.156705 / std.dev. = 0.546463
[23:06:49] [Distribution fitting] Gauss: Chi2 Distance = 32961.4
[23:06:49] Scalar field RMS = 1.27929
![[Pasted image 20230902231549.png]]

### UtahTeapotCircularnoLamp_70_0.5_1.5
SfM = 85423
Alignment => [00:51:10] [Register] Final RMS*: 3.17003 (computed on 49948 points)
[00:52:07] [Distribution fitting] Gauss: mean = 1.028519 / std.dev. = 0.744866
[00:52:07] [Distribution fitting] Gauss: Chi2 Distance = 5481.56
[00:52:07] Scalar field RMS = 1.26991
![[Pasted image 20230903005305.png]]
### UtahTeapotCircularnoLamp_40_0.5_1.5
SfM = 38355
Alignment: [01:21:16] [Register] Final RMS*: 0.582144 (computed on 50000 points)
[01:22:54] [Distribution fitting] Gauss: mean = 0.139583 / std.dev. = 0.253334
[01:22:54] [Distribution fitting] Gauss: Chi2 Distance = 99402.5
[01:22:54] Scalar field RMS = 0.289243
![[Pasted image 20230903012408.png]]
### UtahTeapotCircularLamp_40_0.5_1.5
SfM = 36387
Alignment: [01:17:36] [Register] Final RMS*: 0.551648 (computed on 50000 points)
[01:18:22] [Distribution fitting] Gauss: mean = 0.115447 / std.dev. = 0.321515
[01:18:22] [Distribution fitting] Gauss: Chi2 Distance = 82162.3
[01:18:22] Scalar field RMS = 0.341614
![[Pasted image 20230907011855.png]]

### RandowmShape2SpiralNoLamp_105_1.125_1.5
SfM = 153511
Alignment: [01:56:17] [Register] Final RMS*: 0.447364 (computed on 49831 points)
[01:59:22] [Distribution fitting] Gauss: mean = 0.142764 / std.dev. = 0.094964
[01:59:22] [Distribution fitting] Gauss: Chi2 Distance = 477615
[01:59:22] Scalar field RMS = 0.171464
![[Pasted image 20230907020114.png]]
### RandowmShape2Spiral_105_1.125_1.5
SfM = 148329
Alignment: [02:09:34] [Register] Final RMS*: 0.814562 (computed on 50000 points)
[02:13:36] [Distribution fitting] Gauss: mean = 0.273026 / std.dev. = 0.185674
[02:13:36] [Distribution fitting] Gauss: Chi2 Distance = 237181
[02:13:36] Scalar field RMS = 0.330179
![[Pasted image 20230907021618.png]]
### RandowmShape2CirularNoLamp_105_1.125_1.5
SfM = 130668
[02:28:01] [Register] Final RMS*: 0.487247 (computed on 49746 points)
[02:31:08] [Distribution fitting] Gauss: mean = 0.154860 / std.dev. = 0.141147
[02:31:08] [Distribution fitting] Gauss: Chi2 Distance = 689599
[02:31:08] Scalar field RMS = 0.209533
![[Pasted image 20230907023142.png]]
### RandowmShape2CirularLamp_105_1.125_1.5
SfM = 122687
Alignment : [02:40:07] [Register] (* RMS is potentially weighted, depending on the selected options)
[02:41:24] [Distribution fitting] Gauss: mean = 0.100712 / std.dev. = 0.116869
[02:41:24] [Distribution fitting] Gauss: Chi2 Distance = 1.43358e+06
[02:41:24] Scalar field RMS = 0.154277
![[Pasted image 20230907025702.png]]

### RandowmShape2CirularLamp_60_1.125_2
SfM = 73088
Alignment : [03:08:32] [Register] Final RMS*: 0.183705 (computed on 49767 points)
[03:11:23] [Distribution fitting] Gauss: mean = 0.038678 / std.dev. = 0.119707
[03:11:23] [Distribution fitting] Gauss: Chi2 Distance = 352111
[03:11:23] Scalar field RMS = 0.1258
![[Pasted image 20230907031617.png]]

### RandowmShape2SpiralLamp_105_1.125_2
SfM = 154181
Alignment: [04:10:53] [Register] Final RMS*: 0.26524 (computed on 50000 points)
[04:11:42] [Distribution fitting] Gauss: mean = 0.018419 / std.dev. = 0.119600
[04:11:42] [Distribution fitting] Gauss: Chi2 Distance = 261798
[04:11:42] Scalar field RMS = 0.12101
![[Pasted image 20230907041220.png]]
# Outliers
### RandowmShape2CirularLamp_105_1.125_1
![[Pasted image 20230907030001.png]]

# Conclusion
- Attaching lights to the camera can be harmful. This is probably due to the overexposure of surfaces.
- Too many images may deter the quality of reconstruction, and thus quantity does not always co-relate positively to quality of the reconstruction.
- Spiral patterns perform better than circular patterns in longer distances.
- 