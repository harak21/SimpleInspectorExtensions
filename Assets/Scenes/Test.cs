using System;
using System.Collections.Generic;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.CreationAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.MetaAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StructuralAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.StyleAttributes;
using SimpleUtils.SimpleInspectorExtensions.Core.Attributes.ValidationAttributes;
using UnityEngine;

namespace Scenes
{
    public class Test : MonoBehaviour
    {
        private void Awake()
        {
            listView.Add(this.gameObject);
        }

        [BoxGroup("ListView")]
        public List<GameObject> listView = new List<GameObject>();
        
        [Tag]
        private string Tag;

        [Layer, Label("NewLayerName")] private int Layer;

        [MaxValue(14), MinValue(10.3)] public int dou;
        
        
        [Dropdown("choices"), SerializeField] public string Fooo;

        private List<string> choices = new List<string>() { "a", "b" };
        
        
        [BoxGroup("sadds"), Padding(25), HideIf(nameof(hide))] public bool hiddenElement;
        [BorderWidth(2), BorderColor(InspectorColor.Black), BorderRadius(4), Margin(0,0,0,3)] public bool hide;

        [TextColor(InspectorColor.Gold)] public List<GameObject> List;
        [BoxGroup("sfs"), TextColor(InspectorColor.Gold)] public NestedTest nestedTest;

        [BoxGroup("box")] public Vector3 vector3; 
        
        [BoxGroup("Elements test"), Button("Invoke me")]
        private void ButtonTest()
        {
            Debug.Log("Invoked");
        }

        [BoxGroup("Elements test"), MultilineField, InspectorTooltip("string a")] private string s = "hiddenStr";
        [SerializeField][BoxGroup("Elements test")] private Color _color;
        [SerializeField][BoxGroup("Elements test"), Order(1000, true)] private LayerMask _layerMask;
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
        [SerializeField][BoxGroup("Elements test")] private List<Transform> _list;
        
        [Slider(1,15), Foldout("Slider")] private float _slider;
        [MinMaxSlider(0,100), Foldout("Slider")] private Vector2 _minMaxSlider;
        [ProgressBar, Foldout("Slider")] private float _bar = 35f;

        [RadioButtonGroup("rbg")] private bool _a;
        [RadioButtonGroup("rbg")] private bool _b;
        [RadioButtonGroup("rbg")] private bool _c;

        [OnValueChanged(nameof(OnValueChanged))]
        public int value;

        private void OnValueChanged(int i)
        {
            Debug.Log(i);
        }

        private void OnGoChanged()
        {
            Debug.Log("Collection was changed");
        }

        [AnimatorParameters]
        private string animator;

        [OnValueChanged(nameof(OnGoChanged))]
        private List<GameObject> targetList = new();

        [CustomAdd(nameof(Add))]
        private List<NestedTest> nestedList = new();

        private NestedTest Add()
        {
            return new NestedTest() { integer = 42, sdasd = "text example" };
        }
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