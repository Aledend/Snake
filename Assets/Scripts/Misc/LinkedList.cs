using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedList<TType> : IEnumerator, IEnumerable { 
    class Node
    {
        public TType value;
        public Node nextNode;
        public Node()
        { }

        public Node(TType value)
        {
            this.value = value;
        }

        public Node(Node node)
        {
            value = node.value;
        }

        ~Node()
        {
            Console.WriteLine("Running dTor");
        }
    }

    private Node head;
    private int iteratorPosition = -1;

    public TType this[int index]
    {
        get => GetValue(index);
        set => SetValue(index, value);
    }

    public int Count
    {
        get; private set;
    }

    public object Current => GetValue(iteratorPosition);
    public TType Last => GetLastNode();

    public void SetValue(int index, TType value)
    {
        if (index < 0 || index >= Count || head == null)
        {
            throw new IndexOutOfRangeException();
        }
        Node currentNode = head;
        for (int i = 0; i <= index && currentNode != null; i++)
        {
            if (index == i)
            {
                currentNode.value = value;
                return;
            }
            currentNode = currentNode.nextNode;
        }
    }
    private TType GetValue(int index)
    {
        if (index < 0 || index >= Count || head == null)
        {
            throw new IndexOutOfRangeException();
        }
        Node currentNode = head;
        for (int i = 0; i <= index && currentNode != null; i++)
        {
            if (index == i)
            {
                return currentNode.value;
            }
            currentNode = currentNode.nextNode;
        }
        return currentNode.value;
    }

    public void Push(TType value)
    {
        Node newNode = new Node(value);
        newNode.nextNode = head;
        head = newNode;
        Count++;
    }

    public void InsertAt(int index, TType value)
    {
        if (index < 0 || index > Count)
        {
            throw new IndexOutOfRangeException();
        }

        Node newNode = new Node(value);
        if (index == 0)
        {
            newNode.nextNode = head;
            head = newNode;
            Count++;
            return;
        }

        Node node = GetNode(index - 1);
        newNode.nextNode = node.nextNode;
        node.nextNode = newNode;
        Count++;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index > Count)
        {
            throw new IndexOutOfRangeException();
        }

        if (index == 0)
        {
            head = head.nextNode;
            Count--;
            return;
        }
    }

    private Node GetNode(int index)
    {
        if (index < 0 || index >= Count || head == null)
        {
            throw new IndexOutOfRangeException();
        }

        Node currentNode = head;
        for (int i = 0; i <= index; i++)
        {
            if (currentNode == null)
                throw new IndexOutOfRangeException();

            if (index == i)
            {
                return currentNode;
            }
            currentNode = currentNode.nextNode;
        }
        return null;
    }

    private TType GetLastNode()
    {
        if (Count == 0)
        {
            throw new IndexOutOfRangeException();
        }

        Node currentNode = head;
        while(currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
        }
        return currentNode.value;
    }


    public void Append(TType value)
    {
        Node node = new Node(value);
        if (head == null)
        {
            head = node;
            Count++;
            return;
        }

        Node currentNode = head;
        while (currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
        }
        currentNode.nextNode = node;
        Count++;
    }

    public void RemoveLast()
    {
        Node currentNode = head;
        if(currentNode.nextNode == null)
        {
            head = null;
            Count--;
        }
        else
        {
            while (currentNode.nextNode.nextNode != null)
            {
                currentNode = currentNode.nextNode;
            }
            currentNode.nextNode = null;
            Count--;
        }
        
        
    }

    public bool MoveNext()
    {
        return iteratorPosition++ < Count - 1;
    }

    public void Reset()
    {
        iteratorPosition = -1;
    }

    public void Clear()
    {
        while(Count > 0)
        {
            head = head.nextNode;
            Count--;
        }
    }

    public IEnumerator GetEnumerator()
    {
        return this;
    }
}