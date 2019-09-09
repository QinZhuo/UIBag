using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace QTool.UI
{
    /// <summary>
    /// 背包物体 用来检测指针事件
    /// </summary>
    public class QBagItem : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler
    {
        public QBag bag;
        /// <summary>
        /// 是否可以被点击
        /// </summary>
        public bool RayCastTarget
        {
            set
            {
                GetComponent<MaskableGraphic>().raycastTarget = value;
            }
        }
        public RectTransform rectTransform
        {
            get
            {
                return transform as RectTransform;
            }
        }
        public Vector2 size
        {
            set
            {
                rectTransform.sizeDelta = value;
            }
            get
            {
                return rectTransform.sizeDelta;
            }
        }

        /// <summary>
        /// 点击物体时 开始拖拽
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.LogError("QBagDrag" + name);
            if (!QBag.IsDrag)
            {
                QBag.DragItem(this);
            }
        }
        /// <summary>
        /// 有拖拽物体时 展示虚拟的拖拽物体到此处
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (QBag.IsDrag&&bag!=null)
            {
                bag.VirtualDragEnd(transform.GetSiblingIndex());
            }
           
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public void VirtualHide()
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 显示
        /// </summary>
        public void VirtualShow()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.gameObject.SetActive(true);
            }
        }
        
        // Start is called before the first frame update
        void Start()
        {
            bag = GetComponentInParent<QBag>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
