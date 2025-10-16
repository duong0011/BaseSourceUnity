using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TutorialData", menuName = "DataSoftWare")]
public class TutorialSO : ScriptableObject
{
    [SerializeField] List<TutorialData> m_data;
    public List<TutorialData> Data => m_data;
}
[Serializable]
public struct TutorialData
{
    public string buttonName;
    public string buttonContent;
    public AudioClip buttonAudio;
}