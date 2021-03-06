﻿
using System;
using System.Collections.Generic;

namespace ASD
{

public interface IPriorityQueue
    {
    void Put(int p);     // wstawia element do kolejki
    int GetMax();        // pobiera maksymalny element z kolejki (element jest usuwany z kolejki)
    int ShowMax();       // pokazuje maksymalny element kolejki (element pozostaje w kolejce)
    int Count { get; }   // liczba elementów kolejki
    }


public class LazyPriorityQueue  : MarshalByRefObject, IPriorityQueue
    {

    public LazyPriorityQueue()
        {
        }

    public void Put(int p)
        {
        }

    public int GetMax()
        {
        return 0;
        }

    public int ShowMax()
        {
        return 0;
        }

    public int Count
        {
        get {
            return 0;
            }
        }

    } // LazyPriorityQueue


public class EagerPriorityQueue : MarshalByRefObject, IPriorityQueue
    {

    public EagerPriorityQueue()
        {
        }

    public void Put(int p)
        {
        }

    public int GetMax()
        {
        return 0;
        }

    public int ShowMax()
        {
        return 0;
        }

    public int Count
        {
        get {
            return 0;
            }
        }

    } // EagerPriorityQueue


public class HeapPriorityQueue : MarshalByRefObject, IPriorityQueue
    {

    public HeapPriorityQueue()
        {
        }

    public void Put(int p)
        {
        }

    public int GetMax()
        {
        return 0;
        }

    public int ShowMax()
        {
        return 0;
        }

    public int Count
        {
        get {
            return 0;
            }
        }

    } // HeapPriorityQueue

}
