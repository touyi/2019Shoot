//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class TypeCastHelper : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal TypeCastHelper(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(TypeCastHelper obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~TypeCastHelper() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          TypeCastHelpPINVOKE.delete_TypeCastHelper(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public TypeCastHelper() : this(TypeCastHelpPINVOKE.new_TypeCastHelper(), true) {
  }

  public int CastInt(string buffer) {
    int ret = TypeCastHelpPINVOKE.TypeCastHelper_CastInt(swigCPtr, buffer);
    return ret;
  }

  public void Put(string buffer) {
    TypeCastHelpPINVOKE.TypeCastHelper_Put(swigCPtr, buffer);
  }

  public string Get() {
    string ret = TypeCastHelpPINVOKE.TypeCastHelper_Get(swigCPtr);
    return ret;
  }

}
