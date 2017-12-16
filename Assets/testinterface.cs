using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//the interface
public interface iThing
{
    bool exposedWhatever { get; set; }
    bool exposedAndSetfromEditor { get; set; }
    void DoSomething(int number);
}

//a class that you will access via the interface.

public class SpecificThing : MonoBehaviour, iThing
{
    //Properties can be exposed via Interfaces
    public bool exposedWhatever { get; set; }

    //But a variable cannot be
    public int publicButNotVia_iThing;

    //Too bad you can't set properties with the Unity Editor
    //Just do this:
    public bool _exposedAndSetfromEditor;
    public bool exposedAndSetfromEditor { get { return _exposedAndSetfromEditor; } set { _exposedAndSetfromEditor = value; } }
    //_exposedAndSetfromEditor can be set in the editor and you can work with exposedAndSetfromEditor via the interface

    //work with methods as usual.  
    public void DoSomething(int number)
    {
        publicButNotVia_iThing = number * 1000000;
    }

}

//If you are working with interfaces in unity you are probably getting stuff off of instantiated objects.
//Despite what some "vocal" members of the community claim; you can use GetComponent to retrieve your interfaces.
//I've been doing it since 2010 on PC, Webplayer, and Android - no performance impact and no errors.
//But you do have to use the string overload. It would be nice to get better Interface support from Unity.

//Finally, Code that needed to access different classes via an interface

public class testinterface : MonoBehaviour
{
    public GameObject other;
    public void Start()
    {
        //Okay - getting tired of writing this, use your imagination
        //Obtain your target "other" gameobject that has some script that uses your interface.
        //= GameObject.Find("TheThingGameObject");
        iThing thing = (iThing)other.GetComponent("iThing");
        //And do what you need.
        thing.exposedWhatever = false;
        thing.DoSomething(10);
    }

}
