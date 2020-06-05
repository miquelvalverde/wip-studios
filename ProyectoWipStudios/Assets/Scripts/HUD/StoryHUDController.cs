using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StoryHUDController : MonoBehaivourWithInputs
{
    #region Singleton
    public static StoryHUDController instance;

    protected override void Awake()
    {
        base.Awake();

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }
    #endregion

    private void Start()
    {
        EndStory();
    }

    protected override void SetControls()
    {
        controls.UI.SkipDialog.performed += _ => Next();
        controls.UI.SkipAllDialog.performed += _ => EndStory();
    }

    [SerializeField] private Text textPanel = null;
    [SerializeField] private GameObject panel = null;
    [SerializeField] private Image animalIcon = null;  
    [SerializeField] private Sprite boarIcon = null;
    [SerializeField] private Sprite chameleonIcon = null;
    [SerializeField] private Sprite squirrelIcon = null; 
    [Tooltip("Time between characters. Less is faster.")]
    [SerializeField] private float typeSpeed = 0.01F;
    private bool isTellingStory = false;
    private bool isTyping = false;
    private int index = 0;
    private StoryScriptable currentStory;
        
    public void StartStory(StoryScriptable story)
    {
        if (isTellingStory)
            return;

        if (player.IsDoingSomething())
            return;

        player.DisableInputs();

        isTellingStory = true;
        index = 0;
        currentStory = story;
        animalIcon.sprite = GetImage(story.unlocksAnimal);
        panel.SetActive(true);
        Next();
    }

    private Sprite GetImage(UnlockType unlockType)
    {
        switch (unlockType)
        {
            case UnlockType.BOAR:
                return boarIcon;
            case UnlockType.SQUIRREL:
                return squirrelIcon;
            case UnlockType.CHAMELEON:
                return chameleonIcon;
            case UnlockType.NONE:
            default:
                return null;
        }
    }

    private void Next()
    {
        if (currentStory == null)
            return;

        if (index <= currentStory.sentences.Count)
        {
            if (isTyping)
            {
                EndType();
            }
            else
            {
                if(IsLast())
                {
                    EndStory();
                }
                else
                {
                    StartCoroutine(TypeRoutine(currentStory.sentences[index]));
                    index++;
                }
            }
        }
        else
        {
            EndStory();
        }
    }

    private bool IsLast()
    {
        return index == currentStory.sentences.Count;
    }

    IEnumerator TypeRoutine(string sentence)
    {
        isTyping = true;

        WaitForSeconds wait = new WaitForSeconds(typeSpeed);
        textPanel.text = string.Empty;
        foreach (char letter in sentence.ToCharArray())
        {
            textPanel.text += letter;
            yield return wait;
        }

        isTyping = false;
    }

    private void EndType()
    {
        StopAllCoroutines();
        textPanel.text = currentStory.sentences[index-1];
        isTyping = false;
    }

    public void EndStory()
    {
        if (currentStory == null)
            return;

        player.EnableInputs();
        player.GetRadialMenuController.Unlock(currentStory.unlocksAnimal);
        textPanel.text = string.Empty;
        panel.SetActive(false);
        isTellingStory = false;
        isTyping = false;
        index = 0;
        currentStory = null;
    }
}