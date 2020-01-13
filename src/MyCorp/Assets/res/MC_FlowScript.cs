using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [Name("Card")]
    [Category("MyCorpFlow")]
    [Description("blablablablablabla")]
    public class MC_FlowScript : FlowControlNode
    {
        protected override void RegisterPorts()
        {
            var condition = AddValueInput<Card>("Condition");
            var trueOut = AddFlowOutput("True");
            var falseOut = AddFlowOutput("False");

            AddFlowInput("Entreee", (f) =>
            {
                //TODO
                // SET NEW DIALOG
                // + NEW CHAR IMG
                // + NEW CLIP PLAY
                // +
                // +
                //TODO COROUTINE : NEW SWIPE left/right
            });
        }
    }
}
