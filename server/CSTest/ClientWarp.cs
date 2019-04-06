//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class ClientWarp : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal ClientWarp(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(ClientWarp obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~ClientWarp() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          ClientDLLPINVOKE.delete_ClientWarp(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public ClientWarp() : this(ClientDLLPINVOKE.new_ClientWarp(), true) {
  }

  public int InitClient(string ip, int port) {
    int ret = ClientDLLPINVOKE.ClientWarp_InitClient(swigCPtr, ip, port);
    return ret;
  }

  public int ConnectServer() {
    int ret = ClientDLLPINVOKE.ClientWarp_ConnectServer(swigCPtr);
    return ret;
  }

  public void ExitClient() {
    ClientDLLPINVOKE.ClientWarp_ExitClient(swigCPtr);
  }

  public void SendData(int proto, string content) {
    ClientDLLPINVOKE.ClientWarp_SendData(swigCPtr, proto, content);
  }

  public DataItem PopNextData() {
    DataItem ret = new DataItem(ClientDLLPINVOKE.ClientWarp_PopNextData(swigCPtr), true);
    return ret;
  }

}
