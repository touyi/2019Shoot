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
  
}