cd /d %~dp0
..\pbRelese\bin\protoc.exe --proto_path=.\ --cpp_out=..\server\server\protocol .\Protocol.proto
..\pbRelese\Proto-netGen\protogen.exe -i:.\Protocol.proto -o:..\PCUnityClient\Assets\Scripts\Protocol\GenProtocol\Protocol.cs