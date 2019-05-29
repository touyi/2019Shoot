//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Protocol.proto
namespace Message
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"KeyData")]
  public partial class KeyData : global::ProtoBuf.IExtensible
  {
    public KeyData() {}
    
    private Message.KeyType _key;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"key", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Message.KeyType key
    {
      get { return _key; }
      set { _key = value; }
    }
    private Message.KeyState _keyState;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"keyState", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Message.KeyState keyState
    {
      get { return _keyState; }
      set { _keyState = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"KeyChange")]
  public partial class KeyChange : global::ProtoBuf.IExtensible
  {
    public KeyChange() {}
    
    private readonly global::System.Collections.Generic.List<Message.KeyData> _keyDatas = new global::System.Collections.Generic.List<Message.KeyData>();
    [global::ProtoBuf.ProtoMember(1, Name=@"keyDatas", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Message.KeyData> keyDatas
    {
      get { return _keyDatas; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Command")]
  public partial class Command : global::ProtoBuf.IExtensible
  {
    public Command() {}
    
    private Message.CmdType _ctype;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"ctype", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Message.CmdType ctype
    {
      get { return _ctype; }
      set { _ctype = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CommandList")]
  public partial class CommandList : global::ProtoBuf.IExtensible
  {
    public CommandList() {}
    
    private readonly global::System.Collections.Generic.List<Message.Command> _commandDatas = new global::System.Collections.Generic.List<Message.Command>();
    [global::ProtoBuf.ProtoMember(1, Name=@"commandDatas", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Message.Command> commandDatas
    {
      get { return _commandDatas; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Vec3")]
  public partial class Vec3 : global::ProtoBuf.IExtensible
  {
    public Vec3() {}
    
    private float _x;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"x", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float x
    {
      get { return _x; }
      set { _x = value; }
    }
    private float _y;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"y", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float y
    {
      get { return _y; }
      set { _y = value; }
    }
    private float _z;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"z", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float z
    {
      get { return _z; }
      set { _z = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"VecList")]
  public partial class VecList : global::ProtoBuf.IExtensible
  {
    public VecList() {}
    
    private readonly global::System.Collections.Generic.List<Message.Vec3> _vec = new global::System.Collections.Generic.List<Message.Vec3>();
    [global::ProtoBuf.ProtoMember(1, Name=@"vec", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Message.Vec3> vec
    {
      get { return _vec; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"IPInfo")]
  public partial class IPInfo : global::ProtoBuf.IExtensible
  {
    public IPInfo() {}
    
    private string _ip = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"ip", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string ip
    {
      get { return _ip; }
      set { _ip = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"KeyType")]
    public enum KeyType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"Fire", Value=0)]
      Fire = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Change", Value=1)]
      Change = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"TypeCount", Value=2)]
      TypeCount = 2
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"KeyState")]
    public enum KeyState
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"Down", Value=0)]
      Down = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Up", Value=1)]
      Up = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Click", Value=2)]
      Click = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"StateCount", Value=3)]
      StateCount = 3
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"CmdType")]
    public enum CmdType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"UserIn", Value=0)]
      UserIn = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UserOut", Value=1)]
      UserOut = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GameEnd", Value=2)]
      GameEnd = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GameBegin", Value=3)]
      GameBegin = 3
    }
  
}