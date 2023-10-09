#experiment #data #preliminaries
# Random Object
![[Pasted image 20230913234829.png]]
Common factors is that there are 2 radii of 2R and 1.5R.
There is a lamp attached to the camera head.
48 images are taken for each set of experiments.
In Cloud Compare:
	1. The Ground Truth and Reconstruction are imported.
	2. A registration of the reconstruction is done against the Ground truth.
	3. The reconstruction is then sliced down to **25** units along the Y axis
	4. A second registration is performed
	5. Comparison of the Reconstruction and Ground Truth is performed.
## Experiment 1 : 24x1_8x3
- Outer Radius  24 at 1 elevation(center)
- Inner Radius at 8 images each at 3 elevations.
- SfM = 53893 points
- All Camera Extrinsics were estimated perfectly
- Alignment: [16:04:29] [Register] Final RMS*: 0.599219 (computed on 50000 points)
- [01:40:43] [Distribution fitting] Gauss: mean = -0.000281 / std.dev. = 0.082200 
- [01:40:43] [Distribution fitting] Gauss: Chi2 Distance = 1.66875e+06 
- [01:40:43] Scalar field RMS = 0.0822002
![[Pasted image 20230912014129.png]]
## Experiment 2: 30x1_3x3
- Outer Radius  30 at 1 elevation(center)
- Inner Radius at 6 images each at 3 elevations.
- SfM = 59308 points
- 2 images were off
- ![[Pasted image 20230911164618.png]]
- First Registration: [01:49:42] [Register] Final RMS*: 1.3338 (computed on 50000 points)
- Second Registration: [01:52:35] [Register] Final RMS*: 1.35837 (computed on 50000 points)
- [01:54:22] [Distribution fitting] Gauss: mean = 0.205261 / std.dev. = 0.327769
- [01:54:22] [Distribution fitting] Gauss: Chi2 Distance = 28163.7
- [02:16:07] Scalar field RMS = 0.386735
- ![[Pasted image 20230912015526.png]]
## Experiment 3: 12x1_12x3
- Outer Radius  12 at 1 elevation(center)
- Inner Radius at 12 images each at 3 elevations.
- SfM = 49462
- 47 cameras registered perfectly.
- First Registration: [02:00:56] [Register] Final RMS*: 0.580533 (computed on 49744 points)
- Second Registration: [02:04:09] [Register] Final RMS*: 0.232407 (computed on 49849 points)
- [02:06:26] [Distribution fitting] Gauss: mean = 0.015632 / std.dev. = 0.121000
- [02:06:26] [Distribution fitting] Gauss: Chi2 Distance = 1.06768e+06
- [02:06:26] Scalar field RMS = 0.122005
- ![[Pasted image 20230912020708.png]]
## Experiment 4 : 24x2
- All 48 cameras were reconstructed.
- SfM = 54010
- First Registration: [02:54:25] [Register] Final RMS*: 0.167889 (computed on 50000 points)
- Second Registration: [02:58:33] [Register] Final RMS*: 0.0770632 (computed on 50000 points)
- [03:01:06] [Distribution fitting] Gauss: mean = -0.000250 / std.dev. = 0.077235
- [03:01:06] [Distribution fitting] Gauss: Chi2 Distance = 546442
- [03:01:06] Scalar field RMS = 0.0772351
- ![[Pasted image 20230913030132.png]]
## Experiment 5 : Mix
- This is different from the previous 4 experiments as it does not take into account the radii parameters outlined above.
- The common circumfrence is 2.2R.
- Took 46 images.
- 46 cameras reconstructed
- SfM = 43571
- 32 images were of a cirular pattern, 2.2 Units away and at 2 elevations(each 16)
- The rest of the images were focused on specific features(3 features)
- On Loading the object, orientation in Cloud Compare was set ![[Pasted image 20230913031624.png]]
- First Registration: [03:17:44] [Register] Final RMS*: 0.220657 (computed on 50000 points)
- Second Registration: [03:20:19] [Register] Final RMS*: 0.107313 (computed on 49786 points)
- [03:21:10] [Distribution fitting] Gauss: mean = 0.008294 / std.dev. = 0.068488
- [03:21:10] [Distribution fitting] Gauss: Chi2 Distance = 1.067e+06
- [03:21:10] Scalar field RMS = 0.068988
- ![[Pasted image 20230913032145.png]]

# Utah Teapot
![[Pasted image 20230913234810.png]]
Common factors is that there are 2 radii of 2R and 1.5R.
There is a lamp attached to the camera head.
48 images are taken for each set of experiments, except for the last 2.
In Cloud Compare:
	1. The Ground Truth and Reconstruction are imported.
	2. A registration of the reconstruction is done against the Ground truth.
	3. The reconstruction is then sliced down to **25** units along the Y axis
	4. A second registration is performed
	5. Comparison of the Reconstruction and Ground Truth is performed.
## Experiment 1: 30x1_6x3
- Outer Radius  30 at 1 elevation(center)
- Inner Radius at 6 images each at 3 elevations.
- SfM = 51312 points
- Only 1 Camera was misplaced.
- First Registration: [19:33:36] [Register] Final RMS*: 1.94298 (computed on 50000 points)
- Second Registration: [19:34:59] [Register] Final RMS*: 2.01835 (computed on 49880 points)
- [19:35:39] [Distribution fitting] Gauss: mean = 0.716629 / std.dev. = 0.881231
- [19:35:39] Scalar field RMS = 1.13584
- ![[Pasted image 20230912193659.png]]

## Experiment 2: 24x1_8x3
- Outer Radius  24 at 1 elevation(center)
- Inner Radius at 8 images each at 3 elevations.
- SfM = 41335 points
- 48 cameras
- 4 cameras were misplaced: 10,11,28 and 44
- ![[Pasted image 20230910212007.png]]
- First Registration: [19:17:39] [Register] Final RMS*: 2.91324 (computed on 49997 points)
- Second Registraion: [19:19:03] [Register] Final RMS*: 3.07844 (computed on 49977 points)
- [19:19:03] [Register] Final RMS*: 3.07844 (computed on 49977 points)
- [19:21:55] [Distribution fitting] Gauss: Chi2 Distance = 9100.34
- [19:21:55] Scalar field RMS = 1.40628
![[Pasted image 20230912192250.png]]
- 

## Experiment 3 : 12x1_12x3
- Outer Radius  12 at 1 elevation(center)
- Inner Radius at 8 images each at 3 elevations.
- SfM = 36910 points
- 46 cameras. 2 Outer cameras missing but all the rest are aligned well.
- First registration: [18:46:22] [Register] Final RMS*: 3.01583 (computed on 49815 points)
- Second Registration : [18:56:10] [Register] Final RMS*: 3.11067 (computed on 50000 points)
- [18:57:28] [Distribution fitting] Gauss: mean = 1.172929 / std.dev. = 0.879861
- [18:57:28] [Distribution fitting] Gauss: Chi2 Distance = 23771.6
- [18:57:28] Scalar field RMS = 1.46626
![[Pasted image 20230912185837.png]]
## Experiment4 : 24x2
- Using a Circular Pattern, 24 by 2 images were taken
- SfM Points = 49903
- 48 cameras reconstructed in SfM
- First Registration: [02:39:51] [Register] Final RMS*: 0.206423 (computed on 50000 points)
- Second Registration: [02:41:18] [Register] Final RMS*: 0.185867 (computed on 50000 points)
- [02:42:04] [Distribution fitting] Gauss: mean = -0.031847 / std.dev. = 0.181412
- [02:42:04] [Distribution fitting] Gauss: Chi2 Distance = 83340.8
- [02:42:04] Scalar field RMS = 0.184186
- ![[Pasted image 20230913024245.png]]
## Experiment 5a: Circular-Manual
- 47 images were taken
- A total of 47 cameras were reconstructed
- SfM = 40941 points
- 32 images were taken in a circular pattern of radius 2.2R, 16 for each of the 2 elevations.
- 15 were taken capturing specific features of the model(the handle and the spout)
- First Registration:[14:52:29] [Register] Final RMS*: 0.318613 (computed on 50000 points)
- Second Registration:[14:54:49] [Register] Final RMS*: 0.20282 (computed on 49977 points)
- [14:57:08] [Distribution fitting] Gauss: mean = 0.006695 / std.dev. = 0.205052
- [14:57:08] [Distribution fitting] Gauss: Chi2 Distance = 89655.8
- [14:57:08] Scalar field RMS = 0.205161
- 
## Experiment 5b: Circular-Manual
- 46 images were taken and all of them reconstructed.
- SfM = 43610 points
- 32 images were taken in a circular pattern of radius 2.2R, 16 for each of the 2 elevations.
- 14 were taken capturing specific features of the model(the handle and the spout)
- First Generation: [20:26:47] [Register] Final RMS*: 0.183942 (computed on 49949 points)
- Second GenerationL [20:28:21] [Register] Final RMS*: 0.184937 (computed on 49982 points)
- [20:29:16] [Distribution fitting] Gauss: mean = -0.021083 / std.dev. = 0.172942
- [20:29:16] [Distribution fitting] Gauss: Chi2 Distance = 69870.7
- [20:29:16] Scalar field RMS = 0.174222
![[Pasted image 20230913202944.png]]

## Conclusions
Knowledge of a bodies geometry can allow for adequate camera poses when capturing the object.
With such knowledge, fewer images can be used for reconstruction thereby minimizing computation expenses.
Larger distances between the camera and the object give better results.