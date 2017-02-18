using VRTK;
using UnityEngine;

/*
    Author: Evan Otero
    Date:   Feb 10, 2017
    
    RightControllerAppearence will manage all right controller events, actions, and tooltips.

    TODO: We will also need a controller interact listener (separate class).  See VRTK's example.
    
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

public class RightControllerAppearence : MonoBehaviour
{
    private VRTK_ControllerTooltips tooltips;
    private VRTK_ControllerActions actions;
    private VRTK_ControllerEvents events;
    private bool ignoreEvents;

    private void Start()
    {
        events = GetComponent<VRTK_ControllerEvents>();
        actions = GetComponent<VRTK_ControllerActions>();
        tooltips = GetComponentInChildren<VRTK_ControllerTooltips>();

        // Setup Controller Event Listeners
        events.TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        events.TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

        events.ButtonOnePressed += new ControllerInteractionEventHandler(DoButtonOnePressed);
        events.ButtonOneReleased += new ControllerInteractionEventHandler(DoButtonOneReleased);

        events.GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        events.GripReleased += new ControllerInteractionEventHandler(DoGripReleased);

        events.TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        events.TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);

        tooltips.ToggleTips(false);
        ignoreEvents = true;
    }

    public void toggleTriggerTooltips(bool state)
    {
        tooltips.ToggleTips(state, VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip);
        if (state)
        {
            actions.ToggleHighlightTrigger(true, Color.yellow, 0.5f);
            actions.SetControllerOpacity(0.8f);
        }
        else
        {
            actions.ToggleHighlightTrigger(false);
            actions.SetControllerOpacity(1f);
        }

    }

    public void toggleButtonOneTooltips(bool state)
    {
        tooltips.ToggleTips(state, VRTK_ControllerTooltips.TooltipButtons.ButtonOneTooltip);
        if (state)
        {
            actions.ToggleHighlightButtonOne(true, Color.yellow, 0.5f);
            actions.SetControllerOpacity(0.8f);
        }
        else
        {
            actions.ToggleHighlightButtonOne(false);
            actions.SetControllerOpacity(1f);
        }
    }

    public void toggleGripTooltips(bool state)
    {
        tooltips.ToggleTips(state, VRTK_ControllerTooltips.TooltipButtons.GripTooltip);
        if (state)
        {
            actions.ToggleHighlightGrip(true, Color.yellow, 0.5f);
            actions.SetControllerOpacity(0.8f);
        }
        else
        {
            actions.ToggleHighlightGrip(false);
            actions.SetControllerOpacity(1f);
        }
    }

    public void toggleTouchpadTooltips(bool state)
    {
        tooltips.ToggleTips(state, VRTK_ControllerTooltips.TooltipButtons.TouchpadTooltip);
        if (state)
        {
            actions.ToggleHighlightTouchpad(true, Color.yellow, 0.5f);
            actions.SetControllerOpacity(0.8f);
        }
        else
        {
            actions.ToggleHighlightTouchpad(false);
            actions.SetControllerOpacity(1f);
        }
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (!ignoreEvents)
            toggleTriggerTooltips(true);
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!ignoreEvents)
            toggleTriggerTooltips(false);
    }

    private void DoButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        if (!ignoreEvents)
            toggleButtonOneTooltips(true);
    }

    private void DoButtonOneReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!ignoreEvents)
            toggleButtonOneTooltips(false);
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (!ignoreEvents)
            toggleGripTooltips(true);
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!ignoreEvents)
            toggleGripTooltips(false);
    }

    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (!ignoreEvents)
            toggleTouchpadTooltips(true);
    }

    private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (!ignoreEvents)
            toggleTouchpadTooltips(false);
    }
}