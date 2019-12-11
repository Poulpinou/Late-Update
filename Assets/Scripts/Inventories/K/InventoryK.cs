using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace LateUpdate {
    public class InventoryK : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] Vector2Int inventorySize = new Vector2Int(32, 8);
        [SerializeField] List<Item> defaultItems;
        [SerializeField] Item testItem;
        #endregion

        #region Private Fields
        List<Data> datas = new List<Data>();
        int[,] gridSchema;
        #endregion

        #region Public Methods
        public void AddItem(Item item, int amount = 1)
        {
            if(item.stackLimit > 1) {
                Data data = GetFreeDataForItem(item);

                while(amount > 0)
                {
                    if (data == null)
                    {
                        Vector2Int anchor;
                        if (!TryGetRoomForItem(item, out anchor))
                            throw new Exception("Inventory full");

                        data = new Data(item, anchor);
                    }

                    data.Fill(amount, out amount);
                }
            }
            else
            {
                Vector2Int anchor;
                if (!TryGetRoomForItem(item, out anchor))
                    throw new Exception("Inventory full");

                datas.Add(new Data(item, anchor, 1));
            }
        }
        #endregion

        #region Private Methods
        private Data GetFreeDataForItem(Item item)
        {
            return datas.Where(d => d.Item == item && !d.IsFull).FirstOrDefault();
        }

        private bool TryGetRoomForItem(Item item, out Vector2Int anchor)
        {
            UpdateGridSchema();

            for (int y = 0; y < inventorySize.y - (item.inventorySize.x - 1); y++)
            {
                for (int x = 0; x < inventorySize.x - (item.inventorySize.x - 1); x++)
                {
                    if (gridSchema[x, y] != 0)
                        continue;

                    Vector2Int currentAnchor = new Vector2Int(x, y);
                    bool willBreak = false;

                    for (int iy = currentAnchor.y; iy < item.inventorySize.y + currentAnchor.y; iy++)
                    {
                        for (int ix = currentAnchor.x; ix < item.inventorySize.x + currentAnchor.x; ix++)
                        {
                            if(gridSchema[ix, iy] != 0)
                            {
                                willBreak = true;
                                break;
                            }
                        }
                        if (willBreak)
                            break;
                    }
                    if (!willBreak)
                    {
                        anchor = currentAnchor;
                        return true;
                    }
                }
            }
            anchor = Vector2Int.zero;
            return false;
        }

        private void UpdateGridSchema()
        {
            datas = datas.OrderBy(d => d.Anchor.y).ThenBy(d => d.Anchor.x).ToList();
            gridSchema = new int[inventorySize.x,inventorySize.y];
            for (int i = 0; i < datas.Count; i++)
            {
                foreach(Vector2Int cell in datas[i].Cells) {
                    gridSchema[cell.x, cell.y] = i + 1;
                }
            }
        }

        void AddDefaultItems() {
            if (defaultItems == null || defaultItems.Count == 0)
                return;

            foreach (Item item in defaultItems)
            {
                AddItem(item);
            }
        }
        #endregion

        #region Runtime Methods
        private void Awake()
        {
            AddDefaultItems();

            Test();
        }
        #endregion

        #region Tests
        void Test()
        {
            AddItem(testItem, 40);

            foreach (Data data in datas)
                Debug.Log(data.Anchor);
        }
        #endregion

        #region Internal Classes
        [Serializable]
        public class Data
        {
            Item item;
            Vector2Int[] cells;
            int stackAmount;

            public Item Item => item;
            public Vector2Int[] Cells => cells;
            public int StackAmount => stackAmount;
            public bool IsFull => stackAmount >= item.stackLimit;
            /// <summary>
            /// Anchor is top left corner
            /// </summary>
            public Vector2Int Anchor { get; private set; }

            public Data(Item item, Vector2Int[] cells, int initialAmount = 0)
            {
                this.item = item;

                if(cells.Length != item.inventorySize.x * item.inventorySize.y)
                {
                    throw new Exception("Invalid input cells");
                }

                this.cells = cells;
                Anchor = cells.OrderBy(c => c.x).ThenBy(c => c.y).First();

                stackAmount = initialAmount;

                if(stackAmount > item.stackLimit)
                {
                    throw new Exception(string.Format("Initial exeed stack limit for {0}", item.itemName));
                }
            }

            public Data(Item item, Vector2Int anchor, int initialAmount = 0)
            {
                this.item = item;
                
                Anchor = anchor;
                GenerateCellsFromAnchor();               

                stackAmount = initialAmount;
                if (stackAmount > item.stackLimit)
                {
                    throw new Exception(string.Format("Initial exeed stack limit for {0}", item.itemName));
                }
            }

            public void Fill(int amount, out int left)
            {
                stackAmount += amount;

                if(stackAmount > item.stackLimit)
                {
                    left = stackAmount - item.stackLimit;
                    stackAmount = item.stackLimit;
                }
                else
                {
                    left = 0;
                }
            }

            void GenerateCellsFromAnchor()
            {
                cells = new Vector2Int[item.inventorySize.x * item.inventorySize.y];

                int i = 0;
                for (int x = 0; x < item.inventorySize.x; x++)
                {
                    for (int y = 0; y < item.inventorySize.y; y++)
                    {
                        cells[i] = new Vector2Int(Anchor.x + x, Anchor.y + y);
                        i++;
                    }
                }
            }
        }
        #endregion

        
    }
}
