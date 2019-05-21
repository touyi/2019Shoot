using System;
using System.Collections.Generic;
using Component.Widget;
using GamePlay.Actor;
using NUnit.Framework;
using UnityEngine;
using Wrapper;

namespace Component.Actor
{
    public enum ActorComponentType
    {
        PlayerBehaviorComponent,
        WeapenComponent,
        AttackComponent,
        ActorGameObjectComponent,
        BornPosSetComponent,
        ActorDataComponent,
        UIRootComponent,
    }
    public class ActorBaseComponent : IBaseComponent
    {
        protected WeakRef<IActor> _actor = new WeakRef<IActor>();
        
        protected List<IWidget> _widgets = new List<IWidget>();

        public bool Enable = true;

        public ActorBaseComponent(IActor actor)
        {
            if (actor != null)
            {
                this._actor.Ref = actor;
            }
            else
            {
                Debug.LogError("actor is null !");
                return;
            }
        }

        public void AddWidget(IWidget widget)
        {
            if (widget != null)
            {
                widget.Init(this);
                this._widgets.Add(widget);
            }
        }

        public void RemoveWidget(IWidget widget)
        {
            if (widget != null && this._widgets.Contains(widget))
            {
                widget.Uninit();
                this._widgets.Remove(widget);
            }
        }

        #region 子类关注

        /// <summary>
        /// 做自己数据的初始化 尽量避免获取其他组件的数据
        /// </summary>
        public virtual void Init()
        {
            
        }

        /// <summary>
        /// 所有组件已经初始化完成 可以获取其他组件数据
        /// </summary>
        public virtual void Start()
        {
        }

        public virtual void Uninit()
        {
            for (int i = 0; i < this._widgets.Count; i++)
            {
                this._widgets[i].Uninit();
            }
        }

        public virtual void Update(float deltaTime)
        {
            for (int i = 0; i < _widgets.Count; i++)
            {
                this._widgets[i].Update(deltaTime);
            }
        }
        #endregion
    }
}