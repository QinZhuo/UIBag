using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace QTool.UI
{
    /// <summary>
    /// 背包类
    /// </summary>
    public class QBag : MonoBehaviour,IPointerEnterHandler
    {
        /// <summary>
        /// 是否在拖拽物体
        /// </summary>
        public static bool IsDrag
        {
            get
            {
                return dragData != null;
            }
        }
        /// <summary>
        /// 创建虚拟物体
        /// </summary>
        public QBagItem CreateVirtualItem()
        {
            var item = Instantiate(dragData.fromItem, content);
            item.VirtualHide();
            item.gameObject.SetActive( true);
            return item;
        }
        /// <summary>
        /// 开始拖拽物体
        /// </summary>
        public static void DragItem( QBagItem dragItem) {

            if (dragData == null && dragItem != null)
            {
                var item = Instantiate(dragItem, dragItem.bag.transform.parent);
                item.size = dragItem.size;
                item.RayCastTarget = false;
                dragItem.VirtualHide();
                dragData = new DragData()
                {
                    fromItem = dragItem,
                    dragItem = item,
                };
            }
               
        }
        /// <summary>
        /// 拖拽信息
        /// </summary>
        private static DragData dragData;
        /// <summary>
        /// 内容父节点
        /// </summary>
        private Transform content;
        void Start()
        {
            var grid=GetComponentInChildren<GridLayoutGroup>();
            if (grid != null)
            {
                content = grid.transform;
            }
        }
        /// <summary>
        /// 显示拖拽物体落点
        /// </summary>
        public void VirtualDragEnd(int index)
        {
            if (dragData == null) return;
            if (dragData.fromItem.bag == this)
            {
                if (dragData.fromItem.gameObject.activeSelf)
                {
                    dragData.fromItem.transform.SetSiblingIndex(index);
                }
                else
                {
                    dragData.fromItem.gameObject.SetActive(true);
                    dragData.fromItem.transform.SetSiblingIndex(index);
                   
                }
                if (dragData.toItem != null)
                {
                    dragData.toItem.gameObject.SetActive(false);
                }
            }
            else
            {
                if (dragData.fromItem != null)
                {
                    dragData.fromItem.gameObject.SetActive(false);
                }
                if (dragData.toItem == null)
                {
                    dragData.toItem = CreateVirtualItem();
                    dragData.toItem.transform.SetSiblingIndex(index);
                }
                else
                {
                
                    if (dragData.toItem.bag == this)
                    {
                        dragData.toItem.transform.SetSiblingIndex(index);
                    }
                    else
                    {
                        dragData.toItem.transform.SetParent(content);
                        dragData.toItem.transform.SetSiblingIndex(index);
                    }
                    if (dragData.toItem.gameObject.activeSelf != true)
                    {
                        dragData.toItem.gameObject.SetActive(true);
                    }
                    
                }
               
               
                
            }
        }
        ///// <summary>
        ///// 添加物体
        ///// </summary>
        ///// <param name="itemRoot"></param>
        //public void AddItem(Transform itemRoot)
        //{
        //    var Uis = itemRoot.GetComponentsInChildren<MaskableGraphic>();
        //    foreach (var ui in Uis)
        //    {
        //        ui.raycastTarget = false;
        //    }
        //    var img = itemRoot.GetComponentInChildren<Image>();
            
        //}
        /// <summary>
        /// 更新拖拽物体位置 判断拖拽结束
        /// </summary>
        void Update()
        {
           
            if (dragData != null && dragData.fromItem.bag == this)
            {
               // Debug.Log("drag" + dragData.dragItem.name+" "+dragItem.transform.position);
                dragData.dragItem.transform.position = Input.mousePosition;
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    Debug.LogError("dragEnd" + dragData.dragItem.name);
                 
                    if (dragData.toItem == null || dragData.toItem.gameObject.activeSelf == false)
                    {
                        dragData.fromItem.VirtualShow();
                       
                    }
                    else
                    {
                        if (dragData.toItem.gameObject.activeSelf)
                        {
                            Destroy(dragData.fromItem.gameObject);
                            dragData.toItem.VirtualShow();
                        }
                    }
                    Destroy(dragData.dragItem.gameObject);
                    dragData = null;

                }
            }
        }
        /// <summary>
        /// 拖拽到背包内空位置时 显示落点在最后
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            VirtualDragEnd(transform.childCount);
        }
    }
    /// <summary>
    /// 拖拽信息
    /// </summary>
    public class DragData
    {
        /// <summary>
        /// 拖拽物体
        /// </summary>
        public QBagItem dragItem;
        /// <summary>
        /// 同一背包原始物体
        /// </summary>
        public QBagItem fromItem;
        /// <summary>
        /// 不同背包显示物体
        /// </summary>
        public QBagItem toItem;
    }
}
