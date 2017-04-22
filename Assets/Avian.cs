using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;

public class Avian : MonoBehaviour {

    public static Avian S;
    AvianAnimator animator;

    public AvianAnimation appear;
    public AvianAnimation idle;
    public AvianAnimation talk;
    public AvianAnimation grinny;
    public AvianAnimation pointandtalk;
    public AvianAnimation mmregen;
    public AvianAnimation serious;
    public AvianAnimation silly;
    public AvianAnimation shocked;
    public AvianAnimation wicked;
    public AvianAnimation fingertapping;
    public AvianAnimation wink;
    public AvianAnimation supergrin;

    void Awake() {
        S = this;
        animator = this.GetComponent<AvianAnimator>();
    }
    
    void OnApplicationFocus(bool hasFocus) {
        if (hasFocus)
            OnApplicationGainedFocus();
    }

    public void OnApplicationGainedFocus() {
        TimeSlotData[] slots = TimeManager.S.LoadDayTimeSlots();
        int totalLength = 0;
        foreach (TimeSlotData slot in slots) {
            totalLength += slot.duration;
        }

        string s = "";
        if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 12)
            s += RandomText("Good Morning! ", "I hope you're having a good morning. ");
        else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 16)
            s += RandomText("Good Afternoon! ", "I hope you're having a good afternoon. ");
        else if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour < 24)
            s += RandomText("Good Evening! ", "I hope you're having a good evening. ");
        else
            s += "Welcome Back! ";

        s += "You have " + ColorStartTeal() + slots.Length.ToString() +" events" + ColorEnd() + " on your schedule today.";
        s += " In total, you've got " + ColorStartOrange() + ToHours(totalLength) + " hours" + ColorEnd() + " of stuff to do!";
        s += " You also have " + ColorStartGreen() + SaveTODO.S.GetTODOCount() + " items" + ColorEnd() + " on your TODO list.";

        animator.AnimateOutSpeech(new AvianSpeech(s, appear));
    }

    public void OnBackToCurrentDay() {
        animator.AnimateOutSpeech(new AvianSpeech(RandomText("And we're back where we started.", "Back to reality"), talk));
    }
    public void OnSelectCurrentDay(bool alreadyThere) {
        if (alreadyThere)
            animator.AnimateOutSpeech(new AvianSpeech("I'd set us to the current day, but we're already there...", fingertapping));
        else
            animator.AnimateOutSpeech(new AvianSpeech("Set back to current day.", talk));
    }

    public void OnMasterButton() {
        animator.AnimateOutSpeech(new AvianSpeech("Master button pressed.", wicked));
    }
    public void OnMasterButtonFizzled() {
        animator.AnimateOutSpeech(new AvianSpeech("Master button fizzled out. Great going kid.", fingertapping));
    }

    public void OnForwardDay() {
        animator.AnimateOutSpeech(new AvianSpeech(RandomText("Set forward one day!", "Day Advanced!"), pointandtalk));
    }
    public void OnBackDay() {
        animator.AnimateOutSpeech(new AvianSpeech(RandomText("Set back one day!", "Day Devanced...\n I think that's a word."), pointandtalk));
    }    public void OnSavedTemplate() {
        string dayOfWeek = DateTime.Now.DayOfWeek.ToString();
        animator.AnimateOutSpeech(new AvianSpeech(
            "Template saved! You're future " + ColorStartPink() + dayOfWeek + "s" + ColorEnd() + " will start with these events.", talk));
    }
    public void OnLoadedTemplate() {
        animator.AnimateOutSpeech(new AvianSpeech("Day reset to template. I hope you didn't just accidently throw away your schedule.", wicked));
    }
    public void OnTODOUpdated(int itemsBefore, int itemsAfter) {
        string s = RandomText("Your TODO list was updated, ", "TODO list updated,");
        if (itemsBefore == itemsAfter)
            s += " but it looks like you're still at the same number of items...";
        else if (itemsBefore > itemsAfter)
            s += RandomText(" and there's less to do than there was before.", " and hey, it's a bit smaller than it was before!");
        else
            s += " and it looks like you have even more to do now!";
        s += " You now have " + ColorStartGreen() + itemsAfter + " items" + ColorEnd() + " TODO in total.";
        animator.AnimateOutSpeech(new AvianSpeech(s, talk));
    }

    public void OnEventTapped(int id) {
        DateTime selectedDay = TimeManager.S.GetSelectedDay();
        TimeSlotData[] slotsToday = TimeManager.S.LoadDayTimeSlots(selectedDay);
        int lengthToday = 0;
        foreach (TimeSlotData slot in slotsToday) {
            lengthToday += slot.duration;
        }
        DateTime yesterday = selectedDay;
        yesterday.AddDays(-1);
        TimeSlotData[] slotsYesterday = TimeManager.S.LoadDayTimeSlots(yesterday);
        int lengthYesterday = 0;
        foreach (TimeSlotData slot in slotsYesterday) {
            lengthYesterday += slot.duration;
        }


        // "You have 12 events for Game Dev today, totalling 3 hours. This is more than the 2 hours you had yesterday.
        string s = "";
        if (selectedDay.Date <= DateTime.Now.Date)
            s += "You had ";
        else if (selectedDay.Date >= DateTime.Now.Date)
            s += "You'll have ";
        else
            s += "You have ";

        s += ColorStartTeal() + slotsToday.Length + " events" + ColorEnd() + " for ";
        s += IdToText(id) + " ";

        if (selectedDay.Date != DateTime.Now.Date)
            s += "on that day, ";
        else
            s += "today, ";
        s += "totalling " + ColorStartOrange() + ToHours(lengthToday) + " hours" + ColorEnd() + ". ";

        if (selectedDay.Date <= DateTime.Now.Date)
            s += "The day before you had ";
        else if (selectedDay.Date >= DateTime.Now.Date)
            s += "The before you'll have had ";
        else
            s += "Yesterday you had ";

        s += ColorStartOrange() + ToHours(lengthYesterday) + " hours" + ColorEnd() + ".";

        animator.AnimateOutSpeech(new AvianSpeech(s, talk));
    }
    public void OnEventCreated(int id) {
        string s = "New event created for " + IdToText(id) + ".";
        animator.AnimateOutSpeech(new AvianSpeech(s, talk));
    }
    public void OnEventDeleted(int id) {
        string s = IdToText(id) + " event deleted. Hey you're not procrastinating something, are you?";
        animator.AnimateOutSpeech(new AvianSpeech(s, serious));
    }
    public void OnEventResized(int id, int change) {
        if (change == 0)
            animator.AnimateOutSpeech(new AvianSpeech("Congratulations, you managed to resize your " + IdToText(id) + " event the exact same duration.", fingertapping));
        else if (change > 0)
            animator.AnimateOutSpeech(new AvianSpeech(IdToText(id) + " event duraction increased, look at you Mr. Productive.", talk));
        else
            animator.AnimateOutSpeech(new AvianSpeech(IdToText(id) + " event duraction decreased. You're not slacking, are you?", serious));
    }
    public void OnEventMoved(int id, int change) {
        if (change == 0)
            animator.AnimateOutSpeech(new AvianSpeech("Congratulations, you managed to move your " + IdToText(id) + " event the exact same place.", fingertapping));
        else if (change > 0)
            animator.AnimateOutSpeech(new AvianSpeech(IdToText(id) + " event pushed back. You're not procrastinating, are you?", serious));
        else
            animator.AnimateOutSpeech(new AvianSpeech(IdToText(id) + " event moved up.", talk));
    }
    public void OnAvianTouched(bool withMasterTouch = false) {
        if (withMasterTouch) {
            // Doesn't currently work
            animator.AnimateOutSpeech(new AvianSpeech("Uh, you can't master press me you know...", serious));
        }
        else {
            int r = UnityEngine.Random.Range(0, 3);
            if (r == 0)
                animator.AnimateOutSpeech(new AvianSpeech("?", idle));
            else if (r == 1)
                animator.AnimateOutSpeech(new AvianSpeech("                                                                                                                                              ", grinny));
            else
                animator.AnimateOutSpeech(new AvianSpeech("...", serious));
        }
    }
    public void OnMinuteUpdated() {
        int currentTime = (DateTime.Now.Hour - 7) * 60 + DateTime.Now.Minute;

        TimeSlotData[] slots = TimeManager.S.LoadDayTimeSlots();

        // No events
        if (slots.Length == 0) {
            animator.AnimateOutSpeech(new AvianSpeech("I'm impressed, you have absolutely nothing going on today apparently. Maybe you should add some events", fingertapping));
            return;
        }

        string text = "";
        bool eventJustEnded = false;

        // Event Ending
        foreach (TimeSlotData slot in slots) {
            if (currentTime == slot.startTime + slot.duration) {
                text = RandomText("Congratulations kid, ", "Good work, ");
                text += "you just finished up your " + IdToText(slot.id) + " event. ";
                eventJustEnded = true;

                // Check if another event is starting
                foreach (TimeSlotData slot2 in slots) {
                    if (currentTime == slot2.startTime) {
                        text += " But, it looks like you've got a " + IdToText(slot2.id) + " event up now. Hope you had a good 1 second break.";
                        animator.AnimateOutSpeech(new AvianSpeech(text, wicked));
                        return;
                    }
                }
                
                text += "Now ";
            }
        }

        // Event Starting
        foreach (TimeSlotData slot in slots) {
            if (currentTime == slot.startTime) {
                text = "And with that we've got a " + IdToText(slot.id) + 
                    " event starting now scheduled to last for the next " + ColorStartOrange() + slot.duration + " minutes" + ColorEnd() + ".";
                animator.AnimateOutSpeech(new AvianSpeech(text, talk));
                return;
            }
        }

        if (!eventJustEnded)
            text += RandomText("Heads up, ", "Hey, ", "Just so you know ");

        int closestEventTime = int.MaxValue;
        TimeSlotData closestEvent = new TimeSlotData();
        foreach (TimeSlotData slot in slots) {
            if (currentTime > slot.startTime && currentTime < slot.startTime + slot.duration) {
                text += "you've got " + ColorStartOrange() + (slot.startTime + slot.duration - currentTime) + ColorEnd() + " minutes left until ";
                text += RandomText("you're done with your ", "you're finished with your");
                text += IdToText(slot.id) + " event.";
                animator.AnimateOutSpeech(new AvianSpeech(text, talk));
                return;
            }
            int timeUntil = slot.startTime - currentTime;
            if (timeUntil < closestEventTime) {
                closestEventTime = timeUntil;
                closestEvent = slot;
            }
        }
        if (closestEventTime == int.MaxValue) {
            animator.AnimateOutSpeech(new AvianSpeech("Wow it looks like you've got nothing left to do today, not even sleep. That's suspiciously impressive.", fingertapping));
            return;
        }

        text += "you've got " + ColorStartOrange() + (closestEvent) + ColorEnd() + " minutes left until ";
        text += RandomText("you're done with your ", "you're finished with your");
        text += IdToText(closestEvent.id) + " event.";
        animator.AnimateOutSpeech(new AvianSpeech(text, talk));
        animator.AnimateOutSpeech(new AvianSpeech(text, talk));

    }

    public string ColorStartOrange() {
        return "<color=#ff8000ff>";
    }

    public string ColorStartTeal() {
        return "<color=#00ffffff>";
    }

    public string ColorStartPink() {
        return "<color=#ff00ffff>";
    }
    public string ColorStartGreen() {
        return "<color=#00ff00ff>";
    }
    public string ColorAny(Color color) {
        int red = Mathf.FloorToInt(color.r * 255);
        int green = Mathf.FloorToInt(color.g * 255);
        int blue = Mathf.FloorToInt(color.b * 255);

        char a = GetHex(Mathf.FloorToInt(red / 16));
        char b = GetHex(Mathf.RoundToInt(red % 16));
        char c = GetHex(Mathf.FloorToInt(green / 16));
        char d = GetHex(Mathf.RoundToInt(green % 16));
        char e = GetHex(Mathf.FloorToInt(blue / 16));
        char f = GetHex(Mathf.RoundToInt(blue % 16));

        return "<color=#" + a + b + c + d + e + f + "ff>";
    }
    char GetHex(int i) {
        string alpha = "0123456789ABCDEF";
        return alpha[i];
    }
    public string IdToText(int id) {
        return ColorAny(EventManager.S.events[id].colorText) + EventManager.S.events[id].name + ColorEnd();
    }

    public string ColorEnd() {
        return "</color>";
    }
    public string ToHours(int min) {
        return (min / (float)60).ToString("0.#");
    }
    public string RandomText(params string[] texts) {
        return texts[UnityEngine.Random.Range(0, texts.Length)];
    }
}
