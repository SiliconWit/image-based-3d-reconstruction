from google.protobuf.internal import containers as _containers
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Iterable as _Iterable, Mapping as _Mapping, Optional as _Optional, Union as _Union

DESCRIPTOR: _descriptor.FileDescriptor

class ImgByteArr(_message.Message):
    __slots__ = ("byteArr",)
    BYTEARR_FIELD_NUMBER: _ClassVar[int]
    byteArr: bytes
    def __init__(self, byteArr: _Optional[bytes] = ...) -> None: ...

class QRText(_message.Message):
    __slots__ = ("qrtext",)
    QRTEXT_FIELD_NUMBER: _ClassVar[int]
    qrtext: str
    def __init__(self, qrtext: _Optional[str] = ...) -> None: ...

class QRTexts(_message.Message):
    __slots__ = ("QRTexts",)
    QRTEXTS_FIELD_NUMBER: _ClassVar[int]
    QRTexts: _containers.RepeatedCompositeFieldContainer[QRText]
    def __init__(self, QRTexts: _Optional[_Iterable[_Union[QRText, _Mapping]]] = ...) -> None: ...
