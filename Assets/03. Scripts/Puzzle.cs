using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;
using System;
using UnityEngine.Events;
namespace No
{
    public abstract class Puzzle : MonoBehaviour, IInteractable
    {
        public UnityEvent OnSolved;
        Player owner;
        public Player Owner
        {
            get => owner;
            set => owner = value;
        }

        public abstract void Interact();

    }

}