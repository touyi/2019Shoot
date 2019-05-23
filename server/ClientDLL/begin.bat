swig -csharp -c++ DLLTest.idl
copy /Y ClientDLL.cs ..\..\PCUnityClient\Assets\Scripts\Net\ClientDLL.cs
copy /Y ClientDLLPINVOKE.cs ..\..\PCUnityClient\Assets\Scripts\Net\ClientDLLPINVOKE.cs
copy /Y ClientWarp.cs ..\..\PCUnityClient\Assets\Scripts\Net\ClientWarp.cs
copy /Y DataItem.cs ..\..\PCUnityClient\Assets\Scripts\Net\DataItem.cs