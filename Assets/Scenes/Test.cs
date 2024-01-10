using System;
using System.Collections.Generic;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StructuralAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes;
using UnityEngine;

namespace Scenes
{
    public class Test : MonoBehaviour
    {
        [Dropdown("choices"), SerializeField] public string Fooo;

        private List<string> choices = new List<string>() { "a", "b" };
        
        
        [BoxGroup("sadds"), BackgroundColor(InspectorColor.Red), Padding(25)] public bool notHide;
        [BorderWidth(2), BorderColor(InspectorColor.Black), BorderRadius(4), Margin(0,0,0,3)] public bool hide;

        [LabelColor(InspectorColor.Gold)] public List<GameObject> List;
        [BoxGroup("sfs", InspectorColor.Black, InspectorColor.Red), LabelColor(InspectorColor.Gold)] public NestedTest nestedTest;

        [BoxGroup("box", margin: 15, padding:15, borderWidth: 1, borderColor: InspectorColor.Brown, borderRadius:3)] public Vector3 vector3;
        
        [Button("Invoke me")]
        private void ButtonTest()
        {
            Debug.Log("Invoked");
        }

        [SerializeField][BoxGroup("Elements test", InspectorColor.Palegreen), LabelColor(InspectorColor.Black), HideInInspector] private string s = "hiddenStr";
        [SerializeField][BoxGroup("Elements test"),StyleSheet("TestStyleSheet", true)] private Color _color;
        [SerializeField][BoxGroup("Elements test")] private LayerMask _layerMask;
        [SerializeField][BoxGroup("Elements test")] private EnumA _enumA;
        [SerializeField][BoxGroup("Elements test")] private EnumB _enumB;
        [SerializeField][BoxGroup("Elements test")] private Vector2 _vector2;
        [SerializeField][BoxGroup("Elements test")] private Vector3 _vector3;
        [SerializeField][BoxGroup("Elements test")] private Vector4 _vector4;
        [SerializeField][BoxGroup("Elements test"), HorizontalLine] private Rect _rect;
        [SerializeField][BoxGroup("Elements test")] private char _char;
        [SerializeField][BoxGroup("Elements test")] private AnimationCurve _animationCurve;
        [SerializeField][BoxGroup("Elements test")] private Bounds _bound;
        [SerializeField][BoxGroup("Elements test")] private Gradient _gradient;
        [SerializeField][BoxGroup("Elements test")] private Vector2Int _vector2Int;
        [SerializeField][BoxGroup("Elements test")] private Vector3Int _vector3Int;
        [SerializeField][BoxGroup("Elements test")] private RectInt _rectInt;
        [SerializeField][BoxGroup("Elements test")] private BoundsInt _boundsInt;
        [SerializeField][BoxGroup("Elements test")] private GameObject _gameObject;
        [SerializeField][BoxGroup("Elements test")] private Transform _transform;
        [SerializeField][BoxGroup("Elements test")]
        [BorderRadius(5, true), Margin(5, true), BorderColor(InspectorColor.Gold, true)]private List<Transform> _list;
        
        [LabelColor(InspectorColor.Black), Slider(1,15), Foldout("Slider")] private float _slider;
        [LabelColor(InspectorColor.Black), MinMaxSlider(0,100), Foldout("Slider")] private Vector2 _minMaxSlider;
        [LabelColor(InspectorColor.Black), ProgressBar, Foldout("Slider")] private float _bar = 35f;

        [RadioButtonGroup("rbg")] private bool _a;
        [RadioButtonGroup("rbg")] private bool _b;
        [RadioButtonGroup("rbg")] private bool _c;
    }

    public enum EnumA
    {
        One,
        Two,
        Three
    }

    [Flags]
    public enum EnumB
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 4,
        All = One + Two + Three
    }
}