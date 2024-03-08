from google.protobuf.internal import containers as _containers
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Iterable as _Iterable, Mapping as _Mapping, Optional as _Optional, Union as _Union

DESCRIPTOR: _descriptor.FileDescriptor

class Nil(_message.Message):
    __slots__ = ()
    def __init__(self) -> None: ...

class ImgByteArr(_message.Message):
    __slots__ = ("byteArr",)
    BYTEARR_FIELD_NUMBER: _ClassVar[int]
    byteArr: bytes
    def __init__(self, byteArr: _Optional[bytes] = ...) -> None: ...

class ReferenceVPCount(_message.Message):
    __slots__ = ("refvpcount",)
    REFVPCOUNT_FIELD_NUMBER: _ClassVar[int]
    refvpcount: int
    def __init__(self, refvpcount: _Optional[int] = ...) -> None: ...

class Vec3(_message.Message):
    __slots__ = ("x", "y", "z")
    X_FIELD_NUMBER: _ClassVar[int]
    Y_FIELD_NUMBER: _ClassVar[int]
    Z_FIELD_NUMBER: _ClassVar[int]
    x: float
    y: float
    z: float
    def __init__(self, x: _Optional[float] = ..., y: _Optional[float] = ..., z: _Optional[float] = ...) -> None: ...

class Quat(_message.Message):
    __slots__ = ("w", "x", "y", "z")
    W_FIELD_NUMBER: _ClassVar[int]
    X_FIELD_NUMBER: _ClassVar[int]
    Y_FIELD_NUMBER: _ClassVar[int]
    Z_FIELD_NUMBER: _ClassVar[int]
    w: float
    x: float
    y: float
    z: float
    def __init__(self, w: _Optional[float] = ..., x: _Optional[float] = ..., y: _Optional[float] = ..., z: _Optional[float] = ...) -> None: ...

class DepthData(_message.Message):
    __slots__ = ("min", "max", "median", "mean", "center")
    MIN_FIELD_NUMBER: _ClassVar[int]
    MAX_FIELD_NUMBER: _ClassVar[int]
    MEDIAN_FIELD_NUMBER: _ClassVar[int]
    MEAN_FIELD_NUMBER: _ClassVar[int]
    CENTER_FIELD_NUMBER: _ClassVar[int]
    min: float
    max: float
    median: float
    mean: float
    center: float
    def __init__(self, min: _Optional[float] = ..., max: _Optional[float] = ..., median: _Optional[float] = ..., mean: _Optional[float] = ..., center: _Optional[float] = ...) -> None: ...

class DoneResponse(_message.Message):
    __slots__ = ("done",)
    DONE_FIELD_NUMBER: _ClassVar[int]
    done: bool
    def __init__(self, done: bool = ...) -> None: ...

class CalibrationInput(_message.Message):
    __slots__ = ("position", "image")
    POSITION_FIELD_NUMBER: _ClassVar[int]
    IMAGE_FIELD_NUMBER: _ClassVar[int]
    position: Vec3
    image: ImgByteArr
    def __init__(self, position: _Optional[_Union[Vec3, _Mapping]] = ..., image: _Optional[_Union[ImgByteArr, _Mapping]] = ...) -> None: ...

class NBVInput(_message.Message):
    __slots__ = ("refPose", "image")
    REFPOSE_FIELD_NUMBER: _ClassVar[int]
    IMAGE_FIELD_NUMBER: _ClassVar[int]
    refPose: CamPose
    image: ImgByteArr
    def __init__(self, refPose: _Optional[_Union[CamPose, _Mapping]] = ..., image: _Optional[_Union[ImgByteArr, _Mapping]] = ...) -> None: ...

class CamPose(_message.Message):
    __slots__ = ("location", "orientation", "eulers")
    LOCATION_FIELD_NUMBER: _ClassVar[int]
    ORIENTATION_FIELD_NUMBER: _ClassVar[int]
    EULERS_FIELD_NUMBER: _ClassVar[int]
    location: Vec3
    orientation: Quat
    eulers: Vec3
    def __init__(self, location: _Optional[_Union[Vec3, _Mapping]] = ..., orientation: _Optional[_Union[Quat, _Mapping]] = ..., eulers: _Optional[_Union[Vec3, _Mapping]] = ...) -> None: ...

class CamPoses(_message.Message):
    __slots__ = ("Poses",)
    POSES_FIELD_NUMBER: _ClassVar[int]
    Poses: _containers.RepeatedCompositeFieldContainer[CamPose]
    def __init__(self, Poses: _Optional[_Iterable[_Union[CamPose, _Mapping]]] = ...) -> None: ...
