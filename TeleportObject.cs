using UnityEngine;
using VRTK;

/*
    Author: Evan Otero
    Date:   Feb 10, 2017
    
    TeleportObject is an object in the scene that can be "used", which will result in the player being
    teleported to a specified scene (given in the field in the editor).  Overriding the default use button,
    which is the trigger, improves the process.  This is due to the fact that some scenes are loading quickly,
    which then causes the user to be immediatly teleported to the scene since the trigger hasn't had time to be
    unreleased.

    TeleportObject inherits from VRTK_InteractableObject.
    
    ***********************
    ******* LICENSE *******
    ***********************
    JoyceStick is a Boston College digital humanities project employing Unity
    to construct a virtual reality game from Joyce’s Ulysses for viewing on the
    HTC Vive, supported by a Teaching and Mentoring Grant and substantial funding
    from internal bodies at Boston College.
    Copyright (C) 2017  Evan Otero, Drew Hoo, Emaad Ali, Will Bowditch, Matt Harty, Jake Schafer, & Ryan Reede
    http://joycestick.bc.edu/

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

public class TeleportObject : VRTK_InteractableObject
{
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private bool onUse;
    [SerializeField]
    private GameObject tooltipsObject;
    
    private VRTK_ControllerTooltips tooltips;

    protected void Start()
    {
        this.tooltips = tooltipObject.GetComponent<VRTK_ControllerTooltips>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Grabbed(GameObject usingObject)
    {
        tooltips.triggerText = "Enter " + nextSceneName;

        base.Grabbed(usingObject);
        if (!onUse)
            StartCoroutine(SceneLoader.instance.AsyncLoadScene(nextSceneName));
    }

    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        if (onUse)
            StartCoroutine(SceneLoader.instance.AsyncLoadScene(nextSceneName));
    }

    public override void Ungrabbed(GameObject usingObject) { }

    public override void StopUsing(GameObject usingObject) { }
}
