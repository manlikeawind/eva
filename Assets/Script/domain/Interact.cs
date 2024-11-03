using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interact : MonoBehaviour
{
    public abstract void focus();

    public abstract void blur();

    public abstract void startInteract();

    public abstract void stopInteract();
}
