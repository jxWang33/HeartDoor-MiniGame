using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGoodPhone : TriggerGood
{
    public LoadingPanel loadingPanel;
    public TimeCounter timeCounter;
    protected override void Start() {
        base.Start();
    }

    protected override void TriggerCallBack() {
        base.TriggerCallBack();
        if (timeCounter.minute < 10) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("喂，是小N吗？毕业后这几年混的怎么样啦？国庆节的时候，大家好像都要回学校看看呢，之后正好我们几个一起聚一聚吧！", "小A", null),
                    new DialogueUnit("对啊，对啊。都好久没见了，不知道大家都变成什么样了.", "小B", null),
                    new DialogueUnit("呃....", "小A", null),
                    new DialogueUnit("还是算了吧，我还有好多事情。", "小A", null),
                    new DialogueUnit("这样啊，本来还很期待看到小N的，那还真是遗憾呢...", "小A", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                loadingPanel.Set("Start");
            });
        }
        if (timeCounter.minute < 30) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("喂，是小N吗？毕业后这几年混的怎么样啦？国庆节的时候，大家好像都要回学校看看呢，之后正好我们几个一起聚一聚吧！", "小A", null),
                    new DialogueUnit("对啊，对啊。都好久没见了，不知道大家都变成什么样了.", "小B", null),
                    new DialogueUnit("呃....", "小A", null),
                    new DialogueUnit("还是算了吧，我还有好多事情。", "小A", null),
                    new DialogueUnit("这样啊，本来还很期待看到小N的，那还真是遗憾呢...", "小A", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                loadingPanel.Set("Start");
            });
        }

        else {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("电话挂断了呢....", "小N", null),
                    new DialogueUnit("是接的太晚了吗....", "小N", null),
                    new DialogueUnit("......", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                loadingPanel.Set("Start");
            });
        }
    }
    
    protected override void Update()
    {
        base.Update();
    }
}
