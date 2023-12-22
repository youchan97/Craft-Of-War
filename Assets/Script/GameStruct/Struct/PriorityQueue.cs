using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteBinaryTree<T>
{
    public List<T> array;
    protected int rootNode = 1;
    public CompleteBinaryTree()
    {
        array = new List<T>();
        array.Add(default(T));
    }
    public virtual void Add(T item)
    {
        array.Add(item);
    }
    public virtual T Remove()
    {
        T temp = array[array.Count - 1];
        array.RemoveAt(array.Count - 1);
        return temp;
    }
}
public class MinHeap<T> : CompleteBinaryTree<T> where T : IComparable
{
    public override void Add(T item)
    {
        array.Add(item);
        int inputCurNode = array.Count - 1;
        while (inputCurNode > rootNode)
        {
            int curParantNode = inputCurNode / 2;
            if (array[inputCurNode].CompareTo(curParantNode) > 0)
                break;
            if (curParantNode < rootNode)
                break;
            Swap(inputCurNode, curParantNode);
            inputCurNode = curParantNode;
        }
    }
    public override T Remove()
    {
        T temp = array[rootNode];
        array[rootNode] = array[array.Count - 1];
        array.RemoveAt(array.Count - 1);
        int curparent = rootNode;
        int last = array.Count - 1;
        while (curparent < last)
        {
            int nextChild = curparent * 2;
            if (nextChild < last)
            {
                if (array[nextChild].CompareTo(array[nextChild + 1]) > 0)
                    nextChild++;
            }
            if (nextChild > last)
                break;
            if (array[curparent].CompareTo(array[nextChild]) <= 0)
                break;
            Swap(curparent, nextChild);
            curparent = nextChild;
        }
        return temp;
    }
    public void Swap(int index1, int index2)
    {
        T temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }
}
public interface IPrioxyQueue<T1, T2>
{
    public void Enqueue(T1 item, T2 priority);
    public T1 Dequeue();
    public void Clear();
}
public class PriorityQueue<T1, T2> : MinHeap<T2>, IPrioxyQueue<T1, T2> where T2 : IComparable//T2가 우선순위여서 T2만 비교가능
{
    public Dictionary<T2, T1> dic;//우선순위 넣으면 아이템 나오게
    public PriorityQueue()
    {
        dic = new Dictionary<T2, T1>();
    }
    public void Enqueue(T1 item, T2 priority)
    {
        dic.Add(priority, item);
        Add(priority);
    }
    public T1 Dequeue()
    {
        return dic[Remove()];
    }

    public void Clear()
    {
        array.Clear();
        array.Add(default(T2));
        dic.Clear();
    }
}
