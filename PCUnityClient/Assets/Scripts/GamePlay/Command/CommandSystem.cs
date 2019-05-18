using System;
using GamePlay.Actor;
using UnityEngine;
using Wrapper;

namespace GamePlay.Command
{
    public enum CmdType
    {
        InputCmd,
        UICmd,
        AttackCmd,
        EffectCmd,
    }

    public interface IBaseCommand
    {
        CmdType CmdType { get; }
        bool IsUse { get; set; }
    }

    public class InputCmd : Poolable<InputCmd>, IBaseCommand
    {
        public enum ActionType
        {
            Fire,
            StopFire,
        }

        public CmdType CmdType
        {
            get { return CmdType.InputCmd; }
        }

        public bool IsUse { get; set; }
        public ActionType Action_Type;

        protected override void Init()
        {
            IsUse = false;
        }
    }

    public class UICmd : Poolable<UICmd>, IBaseCommand
    {
        public enum UIType
        {
            EncodeUI,
            HPUI,
            RadarUI,
        }
        public enum UIState
        {
            Close,
            Open,
        }

        public CmdType CmdType
        {
            get { return CmdType.UICmd; }
        }

        public bool IsUse { get; set; }
        public UIType UiType;
        public UIState UiState;
        public string Info;

        protected override void Init()
        {
            IsUse = false;
            Info = "";
        }
    }
    
    public class AttackCmd : Poolable<AttackCmd>, IBaseCommand
    {
        public CmdType CmdType { get; private set; }
        public bool IsUse { get; set; }
        public long SrcActor;
        public long  DesActor;
        public float Demage;
        protected override void Init()
        {
            base.Init();
            SrcActor = 0;
            DesActor = 0;
            this.IsUse = false;
            this.CmdType = CmdType.AttackCmd;
        }
    }
    
    public class EffectCmd : Poolable<EffectCmd>, IBaseCommand
    {
        public CmdType CmdType { get; private set; }
        public bool IsUse { get; set; }
        public string EffectPath;
        public Vector3 PlayWorldPos;

        protected override void Init()
        {
            base.Init();
            this.IsUse = false;
            this.CmdType = CmdType.EffectCmd;
            this.EffectPath = String.Empty;
            this.PlayWorldPos = Vector3.zero;
        }
    }

    public interface IAcceptCommand
    {
        void AcceptCmd(IBaseCommand cmd);
    }
}