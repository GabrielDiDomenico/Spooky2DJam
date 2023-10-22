using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{

    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default;
    [SerializeField] private Sprite _pressed;
    [SerializeField] private AudioClip _compressClip;
    [SerializeField] private AudioClip _jumpScareClip;
    [SerializeField] private AudioSource _source;
    [SerializeField] private Image _jumpScareImage;
    public float timeRemaining = 10;
    
    public void Start()
    {
         _jumpScareImage.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _pressed;
        _source.PlayOneShot(_compressClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _img.sprite = _default;
        // source.PlayOneShot(uncompressClip);
    }

    public void ClickedButton()
    {
        StartCoroutine("ShowJumpscareImage");
    }

    public IEnumerator ShowJumpscareImage()
    {
        _source.PlayOneShot(_jumpScareClip);
        yield return new WaitForSeconds(0.2F);
        _jumpScareImage.enabled = true;
        yield return new WaitForSeconds(0.3F);
        _jumpScareImage.enabled = false;
        SceneManager.LoadScene("Gameplay");
    }

}
