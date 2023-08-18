using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using ControlCatalog.Pages;
using MiniMvvm;

namespace ControlCatalog.ViewModels
{
    public interface ITreeDiagramDataSource
    {
        Node Root { get; }
    }
    
    public class TreeViewPageViewModel : ViewModelBase, ITreeDiagramDataSource
    {
        private readonly Node _root;
        private SelectionMode _selectionMode;

        public TreeViewPageViewModel()
        {
            _root = new Node();

            Items = _root.Children;
            SelectedItems = new ObservableCollection<Node>();

            AddItemCommand = MiniCommand.Create(AddItem);
            RemoveItemCommand = MiniCommand.Create(RemoveItem);
            SelectRandomItemCommand = MiniCommand.Create(SelectRandomItem);

            void Count(ObservableCollection<Node> children)
            {
                foreach (var child in children)
                {
                    NodesCount++;
                    Count(child.Children);
                }
            }

            NodesCount++;
            Count(_root.Children);
        }

        public Node Root => _root;
        public ObservableCollection<Node> Items { get; }
        public ObservableCollection<Node> SelectedItems { get; }
        public MiniCommand AddItemCommand { get; }
        public MiniCommand RemoveItemCommand { get; }
        public MiniCommand SelectRandomItemCommand { get; }
        
        public int NodesCount { get; set; }

        public SelectionMode SelectionMode
        {
            get => _selectionMode;
            set
            {
                SelectedItems.Clear();
                this.RaiseAndSetIfChanged(ref _selectionMode, value);
            }
        }

        private void AddItem()
        {
            var parentItem = SelectedItems.Count > 0 ? (Node)SelectedItems[0] : _root;
            parentItem.AddItem();
        }

        private void RemoveItem()
        {
            while (SelectedItems.Count > 0)
            {
                Node lastItem = (Node)SelectedItems[0];
                RecursiveRemove(Items, lastItem);
                SelectedItems.RemoveAt(0);
            }

            bool RecursiveRemove(ObservableCollection<Node> items, Node selectedItem)
            {
                if (items.Remove(selectedItem))
                {
                    return true;
                }

                foreach (Node item in items)
                {
                    if (item.AreChildrenInitialized && RecursiveRemove(item.Children, selectedItem))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public static readonly Random Random = new Random(10);
        private void SelectRandomItem()
        {
            var depth = Random.Next(4);
            var indexes = Enumerable.Range(0, depth).Select(x => Random.Next(10));
            var node = _root;

            foreach (var i in indexes)
            {
                node = node.Children[i];
            }

            SelectedItems.Clear();
            SelectedItems.Add(node);
        }

        
    }
    
    public class Node
    {
        private const int MaxDepth = 25;
        private readonly int Depth;
        private ObservableCollection<Node>? _children;
        private int _childIndex = 10;

        public Node()
        {
            Header = "Item";
            
            _children = new ObservableCollection<Node>();

            if (Depth < MaxDepth)
            {
                int childCount = (Depth < 3) ? 1 : (Depth < 5) ? 2 : (Depth < 10) ? TreeViewPageViewModel.Random.Next(1, 2) :  TreeViewPageViewModel.Random.Next(0, 2);
                for (int i = 0; i < childCount; i++)
                {
                    Children.Add(new Node(this, i, Depth + 1));
                }
            }
        }

        public Node(Node parent, int index, int depth)
        {
            Depth = depth;
            Parent = parent;
            Header = parent.Header + ' ' + index;
            
            _children = new ObservableCollection<Node>();

            if (Depth < MaxDepth)
            {
                int childCount = Depth switch
                {
                    < 3 => 1,
                    < 5 => 3,
                    < 10 => 1,
                    < 11 => TreeViewPageViewModel.Random.Next(0, 3),
                    < 15 => TreeViewPageViewModel.Random.Next(1, 3),
                    < 20 => 1,
                    < 21 => 2,
                    < 25 => TreeViewPageViewModel.Random.Next(0, 3),
                    < 30 => TreeViewPageViewModel.Random.Next(1, 3),
                    _ => 1
                };
                for (int i = 0; i < childCount; i++)
                {
                    Children.Add(new Node(this, i, Depth + 1));
                }
            }
        }

        public Control Control { get; set; }
        // public SunburstItem SunburstItem => Control as SunburstItem;
        
        public Node? Parent { get; }
        public string Header { get; }
        public bool AreChildrenInitialized => _children != null;
        public ObservableCollection<Node> Children => _children ??= CreateChildren();
        public void AddItem() => Children.Add(new Node(this, _childIndex++, Depth + 1));
        public void RemoveItem(Node child) => Children.Remove(child);
        public override string ToString() => Header;

        private ObservableCollection<Node> CreateChildren()
        {
            if (Depth > 20)
                return new ObservableCollection<Node>();
            var r = new Random();
            return new ObservableCollection<Node>(Enumerable.Range(0, r.Next(1, 3)).Select(i => new Node(this, i, Depth + 1)));
        }
    }
}
