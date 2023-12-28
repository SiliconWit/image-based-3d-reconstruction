from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Mapping as _Mapping, Optional as _Optional, Union as _Union

DESCRIPTOR: _descriptor.FileDescriptor

class Nil(_message.Message):
    __slots__ = ()
    def __init__(self) -> None: ...

class DepthData(_message.Message):
    __slots__ = ("min", "max", "median", "mean", "mode")
    MIN_FIELD_NUMBER: _ClassVar[int]
    MAX_FIELD_NUMBER: _ClassVar[int]
    MEDIAN_FIELD_NUMBER: _ClassVar[int]
    MEAN_FIELD_NUMBER: _ClassVar[int]
    MODE_FIELD_NUMBER: _ClassVar[int]
    min: float
    max: float
    median: float
    mean: float
    mode: float
    def __init__(self, min: _Optional[float] = ..., max: _Optional[float] = ..., median: _Optional[float] = ..., mean: _Optional[float] = ..., mode: _Optional[float] = ...) -> None: ...

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

class ImgByteArr(_message.Message):
    __slots__ = ("byteArr",)
    BYTEARR_FIELD_NUMBER: _ClassVar[int]
    byteArr: bytes
    def __init__(self, byteArr: _Optional[bytes] = ...) -> None: ...

class Vec3(_message.Message):
    __slots__ = ("x", "y", "z")
    X_FIELD_NUMBER: _ClassVar[int]
    Y_FIELD_NUMBER: _ClassVar[int]
    Z_FIELD_NUMBER: _ClassVar[int]
    x: float
    y: float
    z: float
    def __init__(self, x: _Optional[float] = ..., y: _Optional[float] = ..., z: _Optional[float] = ...) -> None: ...
