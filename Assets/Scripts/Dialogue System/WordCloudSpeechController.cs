using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordCloudSpeechController : MonoBehaviour
{
    [SerializeField] private WordcloudSpeech _wordcloud;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _interval;
    [SerializeField] private bool _loop;
    private int index;

    void Start()
    {
        Play();
    }

    void Play()
    {
        index = 0;
        _text.gameObject.SetActive(true);
        StartCoroutine("GetNextPhrase");
    }

    void Stop()
    {
        StopCoroutine("GetNextPhrase");
        _text.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Stop();
    }

    private IEnumerator GetNextPhrase()
    {
        while (index < _wordcloud._phrases.Length)
        {
            _text.text = _wordcloud._phrases[index]._text;
            yield return new WaitForSeconds(_interval);
            if (index == _wordcloud._phrases.Length - 1)
            {
                if (_loop)
                {
                    index = 0;
                } else
                {
                    index++;
                }
            } else
            {
                index++;
            }
        }
        _text.gameObject.SetActive(false);
    }
}
