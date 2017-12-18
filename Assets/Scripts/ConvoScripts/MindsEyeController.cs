using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindsEyeController : MonoBehaviour
{
    Conversation _savedConversation;
    int _savedConvoIndex;
    bool _mindsEyeActive;
    TextPrinter _textPrinter;
    ConversationController _convoController;
    public bool IsActive { get { return _mindsEyeActive; } }
    bool _lockControls;

    TextAsset _xml;
    ConversationContainer _convoContainer;
    Dictionary<string, Conversation> _conversationDictionary = new Dictionary<string, Conversation>();

    void Start()
    {
        _textPrinter = GetComponent<TextPrinter>();
        _convoController = GetComponent<ConversationController>();
        //load convo container
        //_xml = Resources.Load("mindsEyeConvos") as TextAsset;
        //_convoContainer = ConversationContainer.Load(_xml);
        foreach (Conversation convo in _convoContainer.conversationList)
        {
           // _conversationDictionary.Add(convo._name, convo);
        }
    }

    public void startMindsEye(Conversation savedConvo, int savedConvoIndex)
    {
        _mindsEyeActive = true;
        _savedConversation = savedConvo;
        _savedConvoIndex = savedConvoIndex;
        _lockControls = true;
        //play effects
        StartCoroutine("playStartingEffects");
    }

    public void endMindsEye()
    {
        //bring up notification

        _mindsEyeActive = false;
        _convoController.setConversation(_savedConversation, _savedConvoIndex);
        _convoController.loadConversation();
    }

    IEnumerator playStartingEffects()
    {
        //upon activation, play effects
        StartCoroutine(CameraEffects.startFadeRoutine("White"));
        while (CameraEffects.currentFadeAlpha() < .95f)
            yield return new WaitForSeconds(0.1f);
        StartCoroutine("endStartingEffects");
        //we need above coroutine to return a bool for when the effects are done. 
        //This should all probably be in another state
        //look into substates later
        //if minds eye is active, no other action should be allowed

        //See if blurb has retrievable image (true or false)
        //if true, add image to collection as evidence
        //image info by id is pulled from convoOutput
        //if false, return default fail message
        //Keep convo blurb open, do not advance to the next one
        //Keep track of how many uses player has and what has been investigated or not
        yield return null;
    }

    IEnumerator endStartingEffects()
    {
        StartCoroutine(CameraEffects.clearFadeRoutine("White"));
        while (CameraEffects.currentFadeAlpha() > .005f)
            yield return new WaitForSeconds(0.1f);
        //temp
        _mindsEyeActive = false;
        //call function to start convo
        //pull from convo container, make that current convo
        _lockControls = false;

        yield return null;
    }

    public void updateController()
    {
        if (!_lockControls)
        {
            /*if (Input.GetKeyDown(KeyCode.J))
            {
                if (_textPrinter.NumberOfLettersToShow < _textPrinter.TextToType.Length - 1)
                    _textPrinter.NumberOfLettersToShow = _textPrinter.TextToType.Length - 1;
                else
                {
                    endMindsEye();
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ChangeGameState(GameState._historyPauseState);
            }*/
        }
    }
}
