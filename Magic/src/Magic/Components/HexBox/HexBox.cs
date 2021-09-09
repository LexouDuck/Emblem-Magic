using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;
using System.Text;
using System.Collections.Generic;

namespace Magic.Components
{
	/// <summary>
	/// Represents a hex box control.
	/// </summary>
	public class HexBox : Control
	{
		#region Fields

		/// <summary>
		/// Contains the hole content bounds of all text
		/// </summary>
		Rectangle _recContent;
		/// <summary>
		/// Contains the line info bounds
		/// </summary>
		Rectangle _recLineInfo;
		/// <summary>
		/// Contains the column info header rectangle bounds
		/// </summary>
		Rectangle _recColumnInfo;
		/// <summary>
		/// Contains the hex data bounds
		/// </summary>
		Rectangle _recHex;
		/// <summary>
		/// Contains the string view bounds
		/// </summary>
		Rectangle _recStringView;

		/// <summary>
		/// Contains string format information for text drawing
		/// </summary>
		StringFormat _stringFormat;
        /// <summary>
        /// Contains the maximum of visible horizontal bytes
        /// </summary>
        Int32 _iHexMaxHBytes;
        /// <summary>
        /// Contains the maximum of visible vertical bytes
        /// </summary>
        Int32 _iHexMaxVBytes;
        /// <summary>
        /// Contains the maximum of visible bytes.
        /// </summary>
        Int32 _iHexMaxBytes;

        /// <summary>
        /// Contains the scroll bars minimum value
        /// </summary>
        Int64 _scrollVmin;
        /// <summary>
        /// Contains the scroll bars maximum value
        /// </summary>
        Int64 _scrollVmax;
        /// <summary>
        /// Contains the scroll bars current position
        /// </summary>
        Int64 _scrollVpos;
		/// <summary>
		/// Contains a vertical scroll
		/// </summary>
		VScrollBar _vScrollBar;
		/// <summary>
		/// Contains a timer for thumbtrack scrolling
		/// </summary>
		Timer _thumbTrackTimer = new Timer();
        /// <summary>
        /// Contains the thumbtrack scrolling position
        /// </summary>
        Int64 _thumbTrackPosition;
		/// <summary>
		/// Contains the thumptrack delay for scrolling in milliseconds.
		/// </summary>
		const Int32 THUMPTRACKDELAY = 50;
        /// <summary>
        /// Contains the Enviroment.TickCount of the last refresh
        /// </summary>
        Int32 _lastThumbtrack;
        /// <summary>
        /// Contains the border큦 left shift
        /// </summary>
        Int32 _recBorderLeft = SystemInformation.Border3DSize.Width;
        /// <summary>
        /// Contains the border큦 right shift
        /// </summary>
        Int32 _recBorderRight = SystemInformation.Border3DSize.Width;
        /// <summary>
        /// Contains the border큦 top shift
        /// </summary>
        Int32 _recBorderTop = SystemInformation.Border3DSize.Height;
        /// <summary>
        /// Contains the border bottom shift
        /// </summary>
        Int32 _recBorderBottom = SystemInformation.Border3DSize.Height;

        /// <summary>
        /// Contains the index of the first visible byte
        /// </summary>
        Int64 _startByte;
        /// <summary>
        /// Contains the index of the last visible byte
        /// </summary>
        Int64 _endByte;

        /// <summary>
        /// Contains the current byte position
        /// </summary>
        Int64 _bytePos = -1;
        /// <summary>
        /// Contains the current char position in one byte
        /// </summary>
        /// <example>
        /// "1A"
        /// "1" = char position of 0
        /// "A" = char position of 1
        /// </example>
        Int32 _byteCharacterPos;

        /// <summary>
        /// Contains string format information for hex values
        /// </summary>
        String _hexStringFormat = "X";


		/// <summary>
		/// Contains the current key interpreter
		/// </summary>
		IKeyInterpreter _keyInterpreter;
		/// <summary>
		/// Contains an empty key interpreter without functionality
		/// </summary>
		EmptyKeyInterpreter _eki;
		/// <summary>
		/// Contains the default key interpreter
		/// </summary>
		KeyInterpreter _ki;
		/// <summary>
		/// Contains the string key interpreter
		/// </summary>
		StringKeyInterpreter _ski;

        /// <summary>
        /// Contains True if caret is visible
        /// </summary>
        Boolean _caretVisible;

        /// <summary>
        /// Contains true, if the find (Find method) should be aborted.
        /// </summary>
        Boolean _abortFind;
        /// <summary>
        /// Contains a value of the current finding position.
        /// </summary>
        Int64 _findingPos;

        /// <summary>
        /// Contains a state value about Insert or Write mode. When this value is true and the ByteProvider SupportsInsert is true bytes are inserted instead of overridden.
        /// </summary>
        Boolean _insertActive;
        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets the background color for the disabled control.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Color), "WhiteSmoke")]
        public Color BackColorDisabled
        {
            get
            {
                return _backColorDisabled;
            }
            set
            {
                _backColorDisabled = value;
            }
        }
        Color _backColorDisabled = Color.FromName("WhiteSmoke");

        /// <summary>
        /// Gets or sets if the count of bytes in one line is fix.
        /// </summary>
        /// <remarks>
        /// When set to True, BytesPerLine property determine the maximum count of bytes in one line.
        /// </remarks>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets if the count of bytes in one line is fix.")]
        public Boolean ReadOnly
        {
            get { return _readOnly; }
            set
            {
                if (_readOnly == value)
                    return;

                _readOnly = value;
                OnReadOnlyChanged(EventArgs.Empty);
                Invalidate();
            }
        }
        Boolean _readOnly;

        /// <summary>
        /// Gets or sets the maximum count of bytes in one line.
        /// </summary>
        /// <remarks>
        /// UseFixedBytesPerLine property no longer has to be set to true for this to work
        /// </remarks>
        [DefaultValue(16), Category("Hex"), Description("Gets or sets the maximum count of bytes in one line.")]
        public Int32 BytesPerLine
        {
            get { return _bytesPerLine; }
            set
            {
                if (_bytesPerLine == value)
                    return;

                _bytesPerLine = value;
                OnBytesPerLineChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }
        Int32 _bytesPerLine = 16;

        /// <summary>
        /// Gets or sets the number of bytes in a group. Used to show the group separator line (if GroupSeparatorVisible is true)
        /// </summary>
        /// <remarks>
        /// GroupSeparatorVisible property must set to true
        /// </remarks>
        [DefaultValue(4), Category("Hex"), Description("Gets or sets the byte-count between group separators (if visible).")]
        public Int32 GroupSize
        {
            get { return _groupSize; }
            set
            {
                if (_groupSize == value)
                    return;

                _groupSize = value;
                OnGroupSizeChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }
        Int32 _groupSize = 4;
        /// <summary>
        /// Gets or sets if the count of bytes in one line is fix.
        /// </summary>
        /// <remarks>
        /// When set to True, BytesPerLine property determine the maximum count of bytes in one line.
        /// </remarks>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets if the count of bytes in one line is fix.")]
        public Boolean UseFixedBytesPerLine
        {
            get { return _useFixedBytesPerLine; }
            set
            {
                if (_useFixedBytesPerLine == value)
                    return;

                _useFixedBytesPerLine = value;
                OnUseFixedBytesPerLineChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }
        Boolean _useFixedBytesPerLine;

        /// <summary>
        /// Gets or sets the visibility of a vertical scroll bar.
        /// </summary>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of a vertical scroll bar.")]
        public Boolean VScrollBarVisible
        {
            get { return this._vScrollBarVisible; }
            set
            {
                if (_vScrollBarVisible == value)
                    return;

                _vScrollBarVisible = value;

                if (_vScrollBarVisible)
                    Controls.Add(_vScrollBar);
                else
                    Controls.Remove(_vScrollBar);

                UpdateRectanglePositioning();
                UpdateScrollSize();

                OnVScrollBarVisibleChanged(EventArgs.Empty);
            }
        }
        Boolean _vScrollBarVisible;

        /// <summary>
        /// Gets or sets the ByteProvider.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IByteProvider ByteProvider
        {
            get { return _byteProvider; }
            set
            {
                if (_byteProvider == value)
                    return;

                if (value == null)
                    ActivateEmptyKeyInterpreter();
                else
                    ActivateKeyInterpreter();

                if (_byteProvider != null)
                    _byteProvider.LengthChanged -= new EventHandler(_byteProvider_LengthChanged);

                _byteProvider = value;
                if (_byteProvider != null)
                    _byteProvider.LengthChanged += new EventHandler(_byteProvider_LengthChanged);

                OnByteProviderChanged(EventArgs.Empty);

                if (value == null) // do not raise events if value is null
                {
                    _bytePos = -1;
                    _byteCharacterPos = 0;
                    _selectionLength = 0;

                    DestroyCaret();
                }
                else
                {
                    SetPosition(0, 0);
                    SetSelectionLength(0);

                    if (_caretVisible && Focused)
                        UpdateCaret();
                    else
                        CreateCaret();
                }

                CheckCurrentLineChanged();
                CheckCurrentPositionInLineChanged();

                _scrollVpos = 0;

                UpdateVisibilityBytes();
                UpdateRectanglePositioning();

                Invalidate();
            }
        }

        IByteProvider _byteProvider;
        /// <summary>
        /// Gets or sets the visibility of the group separator.
        /// </summary>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of a separator vertical line.")]
        public Boolean GroupSeparatorVisible
        {
            get { return _groupSeparatorVisible; }
            set
            {
                if (_groupSeparatorVisible == value)
                    return;

                _groupSeparatorVisible = value;
                OnGroupSeparatorVisibleChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }
        Boolean _groupSeparatorVisible = false;

        /// <summary>
        /// Gets or sets the visibility of the column info
        /// </summary>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of header row.")]
        public Boolean ColumnInfoVisible
        {
            get { return _columnInfoVisible; }
            set
            {
                if (_columnInfoVisible == value)
                    return;

                _columnInfoVisible = value;
                OnColumnInfoVisibleChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }
        Boolean _columnInfoVisible = false;

        /// <summary>
        /// Gets or sets the visibility of a line info.
        /// </summary>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of a line info.")]
        public Boolean LineInfoVisible
        {
            get { return _lineInfoVisible; }
            set
            {
                if (_lineInfoVisible == value)
                    return;

                _lineInfoVisible = value;
                OnLineInfoVisibleChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }
        Boolean _lineInfoVisible = false;

        /// <summary>
        /// Gets or sets the offset of a line info.
        /// </summary>
        [DefaultValue((Int64)0), Category("Hex"), Description("Gets or sets the offset of the line info.")]
        public Int64 LineInfoOffset
        {
            get { return _lineInfoOffset; }
            set
            {
                if (_lineInfoOffset == value)
                    return;

                _lineInfoOffset = value;

                Invalidate();
            }
        }
        Int64 _lineInfoOffset = 0;

        /// <summary>
        /// Gets or sets the hex box큦 border style.
        /// </summary>
        [DefaultValue(typeof(BorderStyle), "Fixed3D"), Category("Hex"), Description("Gets or sets the hex box큦 border style.")]
        public BorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                if (_borderStyle == value)
                    return;

                _borderStyle = value;
                switch (_borderStyle)
                {
                    case BorderStyle.None:
                        _recBorderLeft = _recBorderTop = _recBorderRight = _recBorderBottom = 0;
                        break;
                    case BorderStyle.Fixed3D:
                        _recBorderLeft = _recBorderRight = SystemInformation.Border3DSize.Width;
                        _recBorderTop = _recBorderBottom = SystemInformation.Border3DSize.Height;
                        break;
                    case BorderStyle.FixedSingle:
                        _recBorderLeft = _recBorderTop = _recBorderRight = _recBorderBottom = 1;
                        break;
                }

                UpdateRectanglePositioning();

                OnBorderStyleChanged(EventArgs.Empty);

            }
        }
        BorderStyle _borderStyle = BorderStyle.Fixed3D;

        /// <summary>
        /// Gets or sets the visibility of the string view.
        /// </summary>
        [DefaultValue(false), Category("Hex"), Description("Gets or sets the visibility of the string view.")]
        public Boolean StringViewVisible
        {
            get { return _stringViewVisible; }
            set
            {
                if (_stringViewVisible == value)
                    return;

                _stringViewVisible = value;
                OnStringViewVisibleChanged(EventArgs.Empty);

                UpdateRectanglePositioning();
                Invalidate();
            }
        }
        Boolean _stringViewVisible;

        /// <summary>
        /// Gets or sets whether the HexBox control displays the hex characters in upper or lower case.
        /// </summary>
        [DefaultValue(typeof(HexCasing), "Upper"), Category("Hex"), Description("Gets or sets whether the HexBox control displays the hex characters in upper or lower case.")]
        public HexCasing HexCasing
        {
            get
            {
                if (_hexStringFormat == "X")
                    return HexCasing.Upper;
                else
                    return HexCasing.Lower;
            }
            set
            {
                String format;
                if (value == HexCasing.Upper)
                    format = "X";
                else
                    format = "x";

                if (_hexStringFormat == format)
                    return;

                _hexStringFormat = format;
                OnHexCasingChanged(EventArgs.Empty);

                Invalidate();
            }
        }

        /// <summary>
        /// Gets and sets the starting point of the bytes selected in the hex box.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int64 SelectionStart
        {
            get { return _bytePos; }
            set
            {
                SetPosition(value, 0);
                ScrollByteIntoView();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets and sets the number of bytes selected in the hex box.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int64 SelectionLength
        {
            get { return _selectionLength; }
            set
            {
                SetSelectionLength(value);
                ScrollByteIntoView();
                Invalidate();
            }
        }
        Int64 _selectionLength;


        /// <summary>
        /// Gets or sets the info color used for column info and line info. When this property is null, then ForeColor property is used.
        /// </summary>
        [DefaultValue(typeof(Color), "Gray"), Category("Hex"), Description("Gets or sets the line info color. When this property is null, then ForeColor property is used.")]
        public Color InfoForeColor
        {
            get { return _infoForeColor; }
            set { _infoForeColor = value; Invalidate(); }
        }
        Color _infoForeColor = Color.Gray;

        /// <summary>
        /// Gets or sets the background color for the selected bytes.
        /// </summary>
        [DefaultValue(typeof(Color), "Blue"), Category("Hex"), Description("Gets or sets the background color for the selected bytes.")]
        public Color SelectionBackColor
        {
            get { return _selectionBackColor; }
            set { _selectionBackColor = value; Invalidate(); }
        }
        Color _selectionBackColor = SystemColors.Highlight;

        /// <summary>
        /// Gets or sets the foreground color for the selected bytes.
        /// </summary>
        [DefaultValue(typeof(Color), "White"), Category("Hex"), Description("Gets or sets the color of the text for the selected bytes.")]
        public Color SelectionForeColor
        {
            get { return _selectionForeColor; }
            set { _selectionForeColor = value; Invalidate(); }
        }
        Color _selectionForeColor = Color.White;

        /// <summary>
        /// Gets or sets the visibility of a shadow selection.
        /// </summary>
        [DefaultValue(true), Category("Hex"), Description("Gets or sets the visibility of a shadow selection.")]
        public Boolean ShadowSelectionVisible
        {
            get { return _shadowSelectionVisible; }
            set
            {
                if (_shadowSelectionVisible == value)
                    return;
                _shadowSelectionVisible = value;
                Invalidate();
            }
        }
        Boolean _shadowSelectionVisible = true;

        /// <summary>
        /// Gets or sets the color of the shadow selection. 
        /// </summary>
        /// <remarks>
        /// A alpha component must be given! 
        /// Default alpha = 100
        /// </remarks>
        [Category("Hex"), Description("Gets or sets the color of the shadow selection.")]
        public Color ShadowSelectionColor
        {
            get { return _shadowSelectionColor; }
            set { _shadowSelectionColor = value; Invalidate(); }
        }
        Color _shadowSelectionColor = Color.FromArgb(100, Color.SkyBlue);

        /// <summary>
        /// Contains the size of a single character in pixel
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SizeF CharSize
        {
            get { return _charSize; }
            private set
            {
                if (_charSize == value)
                    return;
                _charSize = value;
                if (CharSizeChanged != null)
                    CharSizeChanged(this, EventArgs.Empty);
            }
        }
        SizeF _charSize;

        /// <summary>
        /// Gets the width required for the content
        /// </summary>
        [DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int32 RequiredWidth
        {
            get { return _requiredWidth; }
            private set
            {
                if (_requiredWidth == value)
                    return;
                _requiredWidth = value;
                if (RequiredWidthChanged != null)
                    RequiredWidthChanged(this, EventArgs.Empty);
            }
        }
        Int32 _requiredWidth;

        /// <summary>
        /// Gets the number bytes drawn horizontally.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int32 HorizontalByteCount
        {
            get { return _iHexMaxHBytes; }
        }

        /// <summary>
        /// Gets the number bytes drawn vertically.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int32 VerticalByteCount
        {
            get { return _iHexMaxVBytes; }
        }

        /// <summary>
        /// Gets the current line
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int64 CurrentLine
        {
            get { return _currentLine; }
        }
        Int64 _currentLine;

        /// <summary>
        /// Gets the current position in the current line
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int64 CurrentPositionInLine
        {
            get { return _currentPositionInLine; }
        }
        Int32 _currentPositionInLine;

        /// <summary>
        /// Gets the a value if insertion mode is active or not.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean InsertActive
        {
            get { return _insertActive; }
            set
            {
                if (_insertActive == value)
                    return;

                _insertActive = value;

                // recreate caret
                DestroyCaret();
                CreateCaret();

                // raise change event
                OnInsertActiveChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the built-in context menu.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BuiltInContextMenu BuiltInContextMenu
        {
            get { return _builtInContextMenu; }
        }
        BuiltInContextMenu _builtInContextMenu;


        /// <summary>
        /// Gets or sets the converter that will translate between byte and character values.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IByteCharConverter ByteCharConverter
        {
            get
            {
                if (_byteCharConverter == null)
                    _byteCharConverter = new DefaultByteCharConverter();
                return _byteCharConverter;
            }
            set
            {
                if (value != null && value != _byteCharConverter)
                {
                    _byteCharConverter = value;
                    Invalidate();
                }
            }
        }
        IByteCharConverter _byteCharConverter;

        #endregion

        #region Overridden properties
        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        /// <summary>
        /// The font used to display text in the hexbox.
        /// </summary>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                if (value == null)
                    return;

                base.Font = value;
                this.UpdateRectanglePositioning();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Read-only, gets the string of spaced hex currently in the hexbox.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override String Text
        {
            get
            {
                _text = this.ConvertBytesToHex(this.Value);
                return _text;
            }
        }
        private String _text;

        /// <summary>
        /// Gets or sets the byte array contained in the hexbox.
        /// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public Byte[] Value
        {
            get
            {
                _value = ((DataByteProvider)ByteProvider).Bytes.ToArray();
                return _value;
            }
            set
            {
                _value = value;
                ByteProvider = new DataByteProvider(_value);
                return;
            }
        }
        private Byte[] _value;

        /// <summary>
        /// Not used.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Bindable(false)]
        public override RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs, when the value of InsertActive property has changed.
        /// </summary>
        [Description("Occurs, when the value of InsertActive property has changed.")]
		public event EventHandler InsertActiveChanged;
		/// <summary>
		/// Occurs, when the value of ReadOnly property has changed.
		/// </summary>
		[Description("Occurs, when the value of ReadOnly property has changed.")]
		public event EventHandler ReadOnlyChanged;
		/// <summary>
		/// Occurs, when the value of ByteProvider property has changed.
		/// </summary>
		[Description("Occurs, when the value of ByteProvider property has changed.")]
		public event EventHandler ByteProviderChanged;
		/// <summary>
		/// Occurs, when the value of SelectionStart property has changed.
		/// </summary>
		[Description("Occurs, when the value of SelectionStart property has changed.")]
		public event EventHandler SelectionStartChanged;
		/// <summary>
		/// Occurs, when the value of SelectionLength property has changed.
		/// </summary>
		[Description("Occurs, when the value of SelectionLength property has changed.")]
		public event EventHandler SelectionLengthChanged;
		/// <summary>
		/// Occurs, when the value of LineInfoVisible property has changed.
		/// </summary>
		[Description("Occurs, when the value of LineInfoVisible property has changed.")]
		public event EventHandler LineInfoVisibleChanged;
		/// <summary>
		/// Occurs, when the value of ColumnInfoVisibleChanged property has changed.
		/// </summary>
		[Description("Occurs, when the value of ColumnInfoVisibleChanged property has changed.")]
		public event EventHandler ColumnInfoVisibleChanged;
		/// <summary>
		/// Occurs, when the value of GroupSeparatorVisibleChanged property has changed.
		/// </summary>
		[Description("Occurs, when the value of GroupSeparatorVisibleChanged property has changed.")]
		public event EventHandler GroupSeparatorVisibleChanged;
		/// <summary>
		/// Occurs, when the value of StringViewVisible property has changed.
		/// </summary>
		[Description("Occurs, when the value of StringViewVisible property has changed.")]
		public event EventHandler StringViewVisibleChanged;
		/// <summary>
		/// Occurs, when the value of BorderStyle property has changed.
		/// </summary>
		[Description("Occurs, when the value of BorderStyle property has changed.")]
		public event EventHandler BorderStyleChanged;
		/// <summary>
		/// Occurs, when the value of ColumnWidth property has changed.
		/// </summary>
		[Description("Occurs, when the value of GroupSize property has changed.")]
		public event EventHandler GroupSizeChanged;
		/// <summary>
		/// Occurs, when the value of BytesPerLine property has changed.
		/// </summary>
		[Description("Occurs, when the value of BytesPerLine property has changed.")]
		public event EventHandler BytesPerLineChanged;
		/// <summary>
		/// Occurs, when the value of UseFixedBytesPerLine property has changed.
		/// </summary>
		[Description("Occurs, when the value of UseFixedBytesPerLine property has changed.")]
		public event EventHandler UseFixedBytesPerLineChanged;
		/// <summary>
		/// Occurs, when the value of VScrollBarVisible property has changed.
		/// </summary>
		[Description("Occurs, when the value of VScrollBarVisible property has changed.")]
		public event EventHandler VScrollBarVisibleChanged;
		/// <summary>
		/// Occurs, when the value of HexCasing property has changed.
		/// </summary>
		[Description("Occurs, when the value of HexCasing property has changed.")]
		public event EventHandler HexCasingChanged;
		/// <summary>
		/// Occurs, when the value of HorizontalByteCount property has changed.
		/// </summary>
		[Description("Occurs, when the value of HorizontalByteCount property has changed.")]
		public event EventHandler HorizontalByteCountChanged;
		/// <summary>
		/// Occurs, when the value of VerticalByteCount property has changed.
		/// </summary>
		[Description("Occurs, when the value of VerticalByteCount property has changed.")]
		public event EventHandler VerticalByteCountChanged;
		/// <summary>
		/// Occurs, when the value of CurrentLine property has changed.
		/// </summary>
		[Description("Occurs, when the value of CurrentLine property has changed.")]
		public event EventHandler CurrentLineChanged;
		/// <summary>
		/// Occurs, when the value of CurrentPositionInLine property has changed.
		/// </summary>
		[Description("Occurs, when the value of CurrentPositionInLine property has changed.")]
		public event EventHandler CurrentPositionInLineChanged;
		/// <summary>
		/// Occurs, when Copy method was invoked and ClipBoardData changed.
		/// </summary>
		[Description("Occurs, when Copy method was invoked and ClipBoardData changed.")]
		public event EventHandler Copied;
		/// <summary>
		/// Occurs, when CopyHex method was invoked and ClipBoardData changed.
		/// </summary>
		[Description("Occurs, when CopyHex method was invoked and ClipBoardData changed.")]
		public event EventHandler CopiedHex;
        /// <summary>
        /// Occurs, when the CharSize property has changed
        /// </summary>
        [Description("Occurs, when the CharSize property has changed")]
        public event EventHandler CharSizeChanged;
        /// <summary>
        /// Occurs, when the RequiredWidth property changes
        /// </summary>
        [Description("Occurs, when the RequiredWidth property changes")]
        public event EventHandler RequiredWidthChanged;
		#endregion



		#region Ctors

		/// <summary>
		/// Initializes a new instance of a HexBox class.
		/// </summary>
		public HexBox()
		{
			this._vScrollBar = new VScrollBar();
			this._vScrollBar.Scroll += new ScrollEventHandler(_vScrollBar_Scroll);

			this._builtInContextMenu = new BuiltInContextMenu(this);
            this._builtInContextMenu.CopyMenuItemText = "Copy";
            this._builtInContextMenu.CutMenuItemText = "Cut";
            this._builtInContextMenu.PasteMenuItemText = "Paste";
            this._builtInContextMenu.SelectAllMenuItemText = "Select All";

            BackColor = Color.White;
            Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((Byte)(0)));
            _stringFormat = new StringFormat(StringFormat.GenericTypographic);
			_stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

			ActivateEmptyKeyInterpreter();

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);

			_thumbTrackTimer.Interval = 50;
			_thumbTrackTimer.Tick += new EventHandler(PerformScrollThumbTrack);
		}

        #endregion



        #region Caret methods
        void CreateCaret()
        {
            if (_byteProvider == null || _keyInterpreter == null || _caretVisible || !this.Focused)
                return;

            System.Diagnostics.Debug.WriteLine("CreateCaret()", "HexBox");

            // define the caret width depending on InsertActive mode
            Int32 caretWidth = (this.InsertActive) ? 1 : (Int32)_charSize.Width;
            Int32 caretHeight = (Int32)_charSize.Height;
            NativeMethods.CreateCaret(Handle, IntPtr.Zero, caretWidth, caretHeight);

            UpdateCaret();

            NativeMethods.ShowCaret(Handle);

            _caretVisible = true;
        }

        void UpdateCaret()
        {
            if (_byteProvider == null || _keyInterpreter == null)
                return;

            System.Diagnostics.Debug.WriteLine("UpdateCaret()", "HexBox");

            Int64 byteIndex = _bytePos - _startByte;
            PointF p = _keyInterpreter.GetCaretPointF(byteIndex);
            p.X += _byteCharacterPos * _charSize.Width;
            NativeMethods.SetCaretPos((Int32)p.X, (Int32)p.Y);
        }

        void DestroyCaret()
        {
            if (!_caretVisible)
                return;

            System.Diagnostics.Debug.WriteLine("DestroyCaret()", "HexBox");

            NativeMethods.DestroyCaret();
            _caretVisible = false;
        }

        void SetCaretPosition(Point p)
        {
            System.Diagnostics.Debug.WriteLine("SetCaretPosition()", "HexBox");

            if (_byteProvider == null || _keyInterpreter == null)
                return;

            Int64 pos = _bytePos;
            Int32 cp = _byteCharacterPos;

            if (_recHex.Contains(p))
            {
                BytePositionInfo bpi = GetHexBytePositionInfo(p);
                pos = bpi.Index;
                cp = bpi.CharacterPosition;

                SetPosition(pos, cp);

                ActivateKeyInterpreter();
                UpdateCaret();
                Invalidate();
            }
            else if (_recStringView.Contains(p))
            {
                BytePositionInfo bpi = GetStringBytePositionInfo(p);
                pos = bpi.Index;
                cp = bpi.CharacterPosition;

                SetPosition(pos, cp);

                ActivateStringKeyInterpreter();
                UpdateCaret();
                Invalidate();
            }
        }

        BytePositionInfo GetHexBytePositionInfo(Point p)
        {
            System.Diagnostics.Debug.WriteLine("GetHexBytePositionInfo()", "HexBox");

            Int64 bytePos;
            Int32 byteCharaterPos;

            Single x = ((Single)(p.X - _recHex.X) / _charSize.Width);
            Single y = ((Single)(p.Y - _recHex.Y) / _charSize.Height);
            Int32 iX = (Int32)x;
            Int32 iY = (Int32)y;

            Int32 hPos = (iX / 3 + 1);

            bytePos = Math.Min(_byteProvider.Length,
                _startByte + (_iHexMaxHBytes * (iY + 1) - _iHexMaxHBytes) + hPos - 1);
            byteCharaterPos = (iX % 3);
            if (byteCharaterPos > 1)
                byteCharaterPos = 1;

            if (bytePos == _byteProvider.Length)
                byteCharaterPos = 0;

            if (bytePos < 0)
                return new BytePositionInfo(0, 0);
            return new BytePositionInfo(bytePos, byteCharaterPos);
        }

        BytePositionInfo GetStringBytePositionInfo(Point p)
        {
            System.Diagnostics.Debug.WriteLine("GetStringBytePositionInfo()", "HexBox");

            Int64 bytePos;
            Int32 byteCharacterPos;

            Single x = ((Single)(p.X - _recStringView.X) / _charSize.Width);
            Single y = ((Single)(p.Y - _recStringView.Y) / _charSize.Height);
            Int32 iX = (Int32)x;
            Int32 iY = (Int32)y;

            Int32 hPos = iX + 1;

            bytePos = Math.Min(_byteProvider.Length,
                _startByte + (_iHexMaxHBytes * (iY + 1) - _iHexMaxHBytes) + hPos - 1);
            byteCharacterPos = 0;

            if (bytePos < 0)
                return new BytePositionInfo(0, 0);
            return new BytePositionInfo(bytePos, byteCharacterPos);
        }
        #endregion

        #region Scroll methods
        void _vScrollBar_Scroll(Object sender, ScrollEventArgs e)
		{
			switch (e.Type)
			{
				case ScrollEventType.Last:
					break;
				case ScrollEventType.EndScroll:
					break;
				case ScrollEventType.SmallIncrement:
					PerformScrollLineDown();
					break;
				case ScrollEventType.SmallDecrement:
					PerformScrollLineUp();
					break;
				case ScrollEventType.LargeIncrement:
					PerformScrollPageDown();
					break;
				case ScrollEventType.LargeDecrement:
					PerformScrollPageUp();
					break;
				case ScrollEventType.ThumbPosition:
                    Int64 lPos = FromScrollPos(e.NewValue);
					PerformScrollThumpPosition(lPos);
					break;
				case ScrollEventType.ThumbTrack:
					// to avoid performance problems use a refresh delay implemented with a timer
					if (_thumbTrackTimer.Enabled) // stop old timer
						_thumbTrackTimer.Enabled = false;

                    // perform scroll immediately only if last refresh is very old
                    Int32 currentThumbTrack = System.Environment.TickCount;
					if (currentThumbTrack - _lastThumbtrack > THUMPTRACKDELAY)
					{
						PerformScrollThumbTrack(null, null);
						_lastThumbtrack = currentThumbTrack;
						break;
					}

					// start thumbtrack timer 
					_thumbTrackPosition = FromScrollPos(e.NewValue);
					_thumbTrackTimer.Enabled = true;
					break;
				case ScrollEventType.First:
					break;
				default:
					break;
			}

			e.NewValue = ToScrollPos(_scrollVpos);
		}

		/// <summary>
		/// Performs the thumbtrack scrolling after an delay.
		/// </summary>
		void PerformScrollThumbTrack(Object sender, EventArgs e)
		{
			_thumbTrackTimer.Enabled = false;
			PerformScrollThumpPosition(_thumbTrackPosition);
			_lastThumbtrack = Environment.TickCount;
		}

		void UpdateScrollSize()
		{
			System.Diagnostics.Debug.WriteLine("UpdateScrollSize()", "HexBox");

			// calc scroll bar info
			if (VScrollBarVisible && _byteProvider != null && _byteProvider.Length > 0 && _iHexMaxHBytes != 0)
			{
                Int64 scrollmax = (Int64)Math.Ceiling((Double)(_byteProvider.Length + 1) / (Double)_iHexMaxHBytes - (Double)_iHexMaxVBytes);
				scrollmax = Math.Max(0, scrollmax);

                Int64 scrollpos = _startByte / _iHexMaxHBytes;

				if (scrollmax < _scrollVmax)
				{
					/* Data size has been decreased. */
					if (_scrollVpos == _scrollVmax)
						/* Scroll one line up if we at bottom. */
						PerformScrollLineUp();
				}

				if (scrollmax == _scrollVmax && scrollpos == _scrollVpos)
					return;

				_scrollVmin = 0;
				_scrollVmax = scrollmax;
				_scrollVpos = Math.Min(scrollpos, scrollmax);
				UpdateVScroll();
			}
			else if (VScrollBarVisible)
			{
				// disable scroll bar
				_scrollVmin = 0;
				_scrollVmax = 0;
				_scrollVpos = 0;
				UpdateVScroll();
			}
		}

		void UpdateVScroll()
		{
			System.Diagnostics.Debug.WriteLine("UpdateVScroll()", "HexBox");

            Int32 max = ToScrollMax(_scrollVmax);

			if (max > 0)
			{
				_vScrollBar.Minimum = 0;
				_vScrollBar.Maximum = max;
				_vScrollBar.Value = ToScrollPos(_scrollVpos);
				_vScrollBar.Visible = true;
			}
			else
			{
				_vScrollBar.Visible = false;
			}
		}

        Int32 ToScrollPos(Int64 value)
		{
            Int32 max = 65535;

			if (_scrollVmax < max)
				return (Int32)value;
			else
			{
                Double valperc = (Double)value / (Double)_scrollVmax * (Double)100;
                Int32 res = (Int32)Math.Floor((Double)max / (Double)100 * valperc);
				res = (Int32)Math.Max(_scrollVmin, res);
				res = (Int32)Math.Min(_scrollVmax, res);
				return res;
			}
		}

        Int64 FromScrollPos(Int32 value)
		{
            Int32 max = 65535;
			if (_scrollVmax < max)
			{
				return (Int64)value;
			}
			else
			{
                Double valperc = (Double)value / (Double)max * (Double)100;
                Int64 res = (Int32)Math.Floor((Double)_scrollVmax / (Double)100 * valperc);
				return res;
			}
		}

        Int32 ToScrollMax(Int64 value)
		{
            Int64 max = 65535;
			if (value > max)
				return (Int32)max;
			else
				return (Int32)value;
		}

		void PerformScrollToLine(Int64 pos)
		{
			if (pos < _scrollVmin || pos > _scrollVmax || pos == _scrollVpos)
				return;

			_scrollVpos = pos;

			UpdateVScroll();
			UpdateVisibilityBytes();
			UpdateCaret();
			Invalidate();
		}

		void PerformScrollLines(Int32 lines)
		{
            Int64 pos;
			if (lines > 0)
			{
				pos = Math.Min(_scrollVmax, _scrollVpos + lines);
			}
			else if (lines < 0)
			{
				pos = Math.Max(_scrollVmin, _scrollVpos + lines);
			}
			else
			{
				return;
			}

			PerformScrollToLine(pos);
		}

		void PerformScrollLineDown()
		{
			this.PerformScrollLines(1);
		}

		void PerformScrollLineUp()
		{
			this.PerformScrollLines(-1);
		}

		void PerformScrollPageDown()
		{
			this.PerformScrollLines(_iHexMaxVBytes);
		}

		void PerformScrollPageUp()
		{
			this.PerformScrollLines(-_iHexMaxVBytes);
		}

		void PerformScrollThumpPosition(Int64 pos)
		{
            // Bug fix: Scroll to end, do not scroll to end
            Int32 difference = (_scrollVmax > 65535) ? 10 : 9;

			if (ToScrollPos(pos) == ToScrollMax(_scrollVmax) - difference)
				pos = _scrollVmax;
			// End Bug fix


			PerformScrollToLine(pos);
		}

		/// <summary>
		/// Scrolls the selection start byte into view
		/// </summary>
		public void ScrollByteIntoView()
		{
			System.Diagnostics.Debug.WriteLine("ScrollByteIntoView()", "HexBox");

			ScrollByteIntoView(_bytePos);
		}

		/// <summary>
		/// Scrolls the specific byte into view
		/// </summary>
		/// <param name="index">the index of the byte</param>
		public void ScrollByteIntoView(Int64 index)
		{
			System.Diagnostics.Debug.WriteLine("ScrollByteIntoView(long index)", "HexBox");

			if (_byteProvider == null || _keyInterpreter == null)
				return;

			if (index < _startByte)
			{
                Int64 line = (Int64)Math.Floor((Double)index / (Double)_iHexMaxHBytes);
				PerformScrollThumpPosition(line);
			}
			else if (index > _endByte)
			{
                Int64 line = (Int64)Math.Floor((Double)index / (Double)_iHexMaxHBytes);
				line -= _iHexMaxVBytes - 1;
				PerformScrollThumpPosition(line);
			}
		}
		#endregion

		#region Selection methods
		void ReleaseSelection()
		{
			System.Diagnostics.Debug.WriteLine("ReleaseSelection()", "HexBox");

			if (_selectionLength == 0)
				return;
			_selectionLength = 0;
			OnSelectionLengthChanged(EventArgs.Empty);

			if (!_caretVisible)
				CreateCaret();
			else
				UpdateCaret();

			Invalidate();
		}

		/// <summary>
		/// Returns true if Select method could be invoked.
		/// </summary>
		public Boolean CanSelectAll()
		{
			if (!this.Enabled)
				return false;
			if (_byteProvider == null)
				return false;

			return true;
		}

		/// <summary>
		/// Selects all bytes.
		/// </summary>
		public void SelectAll()
		{
			if (this.ByteProvider == null)
				return;
			this.Select(0, this.ByteProvider.Length);
		}

		/// <summary>
		/// Selects the hex box.
		/// </summary>
		/// <param name="start">the start index of the selection</param>
		/// <param name="length">the length of the selection</param>
		public void Select(Int64 start, Int64 length)
		{
			if (this.ByteProvider == null)
				return;
			if (!this.Enabled)
				return;

			InternalSelect(start, length);
			ScrollByteIntoView();
		}

		void InternalSelect(Int64 start, Int64 length)
		{
            Int64 pos = start;
            Int64 sel = length;
            Int32 cp = 0;

			if (sel > 0 && _caretVisible)
				DestroyCaret();
			else if (sel == 0 && !_caretVisible)
				CreateCaret();

			SetPosition(pos, cp);
			SetSelectionLength(sel);

			UpdateCaret();
			Invalidate();
		}
        #endregion

        #region Find methods
        /// <summary>
        /// Searches the current ByteProvider
        /// </summary>
        /// <param name="options">contains all find options</param>
        /// <returns>the SelectionStart property value if find was successfull or
        /// -1 if there is no match
        /// -2 if Find was aborted.</returns>
        public Int64 Find(Byte[] findHex)
        {
            if (findHex == null || findHex.Length == 0)
                throw new ArgumentException("Hex can not be null when Type is Hex");

            return Search(findHex, null);
        }
        public Int64 Find(String findText, Boolean matchCase)
        {
            Byte[] findBuffer = ASCIIEncoding.ASCII.GetBytes(findText);
            Byte[] findBufferLowerCase = ASCIIEncoding.ASCII.GetBytes(findText.ToLower());
            Byte[] findBufferUpperCase = ASCIIEncoding.ASCII.GetBytes(findText.ToUpper());

            if (matchCase)
            {
                if (findBuffer == null || findBuffer.Length == 0)
                    throw new ArgumentException("FindBuffer can not be null when Type: Text and MatchCase: true");

                return Search(findBuffer, null);
            }
            else
            {
                if (findBufferLowerCase == null || findBufferLowerCase.Length == 0)
                    throw new ArgumentException("FindBufferLowerCase can not be null when Type is Text and MatchCase is false");
                if (findBufferUpperCase == null || findBufferUpperCase.Length == 0)
                    throw new ArgumentException("FindBufferUpperCase can not be null when Type is Text and MatchCase is false");
                if (findBufferLowerCase.Length != findBufferUpperCase.Length)
                    throw new ArgumentException("FindBufferUpperCase and FindBufferUpperCase must have the same size when Type is Text and MatchCase is true");

                return Search(findBufferLowerCase, findBufferUpperCase);
            }
        }
        Int64 Search(Byte[] buffer1, Byte[] buffer2)
        {
            var startIndex = this.SelectionStart + this.SelectionLength;
            Int32 match = 0;

            Int32 buffer1Length = buffer1.Length;

            _abortFind = false;

            for (Int64 pos = startIndex; pos < _byteProvider.Length; pos++)
            {
                if (_abortFind)
                    return -2;

                if (pos % 100 == 0) // for performance reasons: DoEvents only 1 times per 100 loops
                    Application.DoEvents();

                Byte compareByte = _byteProvider.ReadByte(pos);
                Boolean buffer1Match = compareByte == buffer1[match];
                Boolean hasBuffer2 = buffer2 != null;
                Boolean buffer2Match = hasBuffer2 ? compareByte == buffer2[match] : false;
                Boolean isMatch = buffer1Match || buffer2Match;
                if (!isMatch)
                {
                    pos -= match;
                    match = 0;
                    _findingPos = pos;
                    continue;
                }

                match++;

                if (match == buffer1Length)
                {
                    Int64 bytePos = pos - buffer1Length + 1;
                    Select(bytePos, buffer1Length);
                    ScrollByteIntoView(_bytePos + _selectionLength);
                    ScrollByteIntoView(_bytePos);

                    return bytePos;
                }
            }

            return -1;
        }

        /// <summary>
        /// Aborts a working Find method.
        /// </summary>
        public void AbortFind()
        {
            _abortFind = true;
        }

        /// <summary>
        /// Gets a value that indicates the current position during Find method execution.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int64 CurrentFindingPosition
        {
            get
            {
                return _findingPos;
            }
        }
        #endregion

        #region Copy, Cut and Paste methods
        Byte[] GetCopyData()
        {
            if (!CanCopy()) return new Byte[0];

            // put bytes into buffer
            Byte[] buffer = new Byte[_selectionLength];
            Int32 id = -1;
            for (Int64 i = _bytePos; i < _bytePos + _selectionLength; i++)
            {
                id++;

                buffer[id] = _byteProvider.ReadByte(i);
            }
            return buffer;
        }
        /// <summary>
        /// Copies the current selection in the hex box to the Clipboard.
        /// </summary>
        public void Copy()
        {
            if (!CanCopy()) return;

            // put bytes into buffer
            Byte[] buffer = GetCopyData();

            DataObject da = new DataObject();

            // set string buffer clipbard data
            String sBuffer = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            da.SetData(typeof(String), sBuffer);

            //set memorystream (BinaryData) clipboard data
            System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer, 0, buffer.Length, false, true);
            da.SetData("BinaryData", ms);

            Clipboard.SetDataObject(da, true);
            UpdateCaret();
            ScrollByteIntoView();
            Invalidate();

            OnCopied(EventArgs.Empty);
        }

        /// <summary>
        /// Return true if Copy method could be invoked.
        /// </summary>
        public Boolean CanCopy()
        {
            if (_selectionLength < 1 || _byteProvider == null)
                return false;

            return true;
        }

        /// <summary>
        /// Moves the current selection in the hex box to the Clipboard.
        /// </summary>
        public void Cut()
        {
            if (!CanCut()) return;

            Copy();

            _byteProvider.DeleteBytes(_bytePos, _selectionLength);
            _byteCharacterPos = 0;
            UpdateCaret();
            ScrollByteIntoView();
            ReleaseSelection();
            Invalidate();
            Refresh();
        }

        /// <summary>
        /// Return true if Cut method could be invoked.
        /// </summary>
        public Boolean CanCut()
        {
            if (ReadOnly || !this.Enabled)
                return false;
            if (_byteProvider == null)
                return false;
            if (_selectionLength < 1 || !_byteProvider.SupportsDeleteBytes())
                return false;

            return true;
        }

        /// <summary>
        /// Replaces the current selection in the hex box with the contents of the Clipboard.
        /// </summary>
        public void Paste()
        {
            if (!CanPaste()) return;

            if (_selectionLength > 0)
                _byteProvider.DeleteBytes(_bytePos, _selectionLength);

            Byte[] data = null;
            IDataObject paste = Clipboard.GetDataObject();
            if (paste.GetDataPresent("BinaryData"))
            {
                System.IO.MemoryStream ms = (System.IO.MemoryStream)paste.GetData("BinaryData");
                data = new Byte[ms.Length];
                ms.Read(data, 0, data.Length);
            }
            else if (paste.GetDataPresent(typeof(String)))
            {
                String sBuffer = (String)paste.GetData(typeof(String));
                try
                {
                    data = Util.SpacedHexToBytes(sBuffer.Trim());
                }
                catch
                {
                    data = Encoding.ASCII.GetBytes(sBuffer);
                }
            }
            else
            {
                return;
            }

            _byteProvider.InsertBytes(_bytePos, data);

            SetPosition(_bytePos + data.Length, 0);

            ReleaseSelection();
            ScrollByteIntoView();
            UpdateCaret();
            Invalidate();
        }

        /// <summary>
        /// Return true if Paste method could be invoked.
        /// </summary>
        public Boolean CanPaste()
        {
            if (ReadOnly || !this.Enabled) return false;

            if (_byteProvider == null || !_byteProvider.SupportsInsertBytes())
                return false;

            if (!_byteProvider.SupportsDeleteBytes() && _selectionLength > 0)
                return false;

            IDataObject da = Clipboard.GetDataObject();
            if (da.GetDataPresent("BinaryData"))
                return true;
            else if (da.GetDataPresent(typeof(String)))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Return true if PasteHex method could be invoked.
        /// </summary>
        public Boolean CanPasteHex()
        {
            if (!CanPaste()) return false;

            Byte[] buffer = null;
            IDataObject da = Clipboard.GetDataObject();
            if (da.GetDataPresent(typeof(String)))
            {
                String hexString = (String)da.GetData(typeof(String));
                buffer = ConvertHexToBytes(hexString);
                return (buffer != null);
            }
            return false;
        }

        /// <summary>
        /// Replaces the current selection in the hex box with the hex string data of the Clipboard.
        /// </summary>
        public void PasteHex()
        {
            if (!CanPaste()) return;

            Byte[] buffer = null;
            IDataObject da = Clipboard.GetDataObject();
            if (da.GetDataPresent(typeof(String)))
            {
                String hexString = (String)da.GetData(typeof(String));
                buffer = ConvertHexToBytes(hexString);
                if (buffer == null)
                    return;
            }
            else
            {
                return;
            }

            if (_selectionLength > 0)
                _byteProvider.DeleteBytes(_bytePos, _selectionLength);

            _byteProvider.InsertBytes(_bytePos, buffer);

            SetPosition(_bytePos + buffer.Length, 0);

            ReleaseSelection();
            ScrollByteIntoView();
            UpdateCaret();
            Invalidate();
        }

        /// <summary>
        /// Copies the current selection in the hex box to the Clipboard in hex format.
        /// </summary>
        public void CopyHex()
        {
            if (!CanCopy()) return;

            // put bytes into buffer
            Byte[] buffer = GetCopyData();

            DataObject da = new DataObject();

            // set string buffer clipbard data
            String hexString = ConvertBytesToHex(buffer); ;
            da.SetData(typeof(String), hexString);

            //set memorystream (BinaryData) clipboard data
            System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer, 0, buffer.Length, false, true);
            da.SetData("BinaryData", ms);

            Clipboard.SetDataObject(da, true);
            UpdateCaret();
            ScrollByteIntoView();
            Invalidate();

            OnCopiedHex(EventArgs.Empty);
        }


        #endregion

        #region PreProcessMessage methods
        /// <summary>
        /// Preprocesses windows messages.
        /// </summary>
        /// <param name="m">the message to process.</param>
        /// <returns>true, if the message was processed</returns>
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
        public override Boolean PreProcessMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_KEYDOWN:
                    return _keyInterpreter.PreProcessWmKeyDown(ref m);
                case NativeMethods.WM_CHAR:
                    return _keyInterpreter.PreProcessWmChar(ref m);
                case NativeMethods.WM_KEYUP:
                    return _keyInterpreter.PreProcessWmKeyUp(ref m);
                default:
                    return base.PreProcessMessage(ref m);
            }
        }

        Boolean BasePreProcessMessage(ref Message m)
        {
            return base.PreProcessMessage(ref m);
        }
        #endregion

        #region Key interpreter methods
        void ActivateEmptyKeyInterpreter()
		{
			if (_eki == null)
				_eki = new EmptyKeyInterpreter(this);

			if (_eki == _keyInterpreter)
				return;

			if (_keyInterpreter != null)
				_keyInterpreter.Deactivate();

			_keyInterpreter = _eki;
			_keyInterpreter.Activate();
		}

		void ActivateKeyInterpreter()
		{
			if (_ki == null)
				_ki = new KeyInterpreter(this);

			if (_ki == _keyInterpreter)
				return;

			if (_keyInterpreter != null)
				_keyInterpreter.Deactivate();

			_keyInterpreter = _ki;
			_keyInterpreter.Activate();
		}

		void ActivateStringKeyInterpreter()
		{
			if (_ski == null)
				_ski = new StringKeyInterpreter(this);

			if (_ski == _keyInterpreter)
				return;

			if (_keyInterpreter != null)
				_keyInterpreter.Deactivate();

			_keyInterpreter = _ski;
			_keyInterpreter.Activate();
		}
        #endregion

        #region IKeyInterpreter interface
        /// <summary>
        /// Defines a user input handler such as for mouse and keyboard input
        /// </summary>
        interface IKeyInterpreter
        {
            /// <summary>
            /// Activates mouse events
            /// </summary>
            void Activate();
            /// <summary>
            /// Deactivate mouse events
            /// </summary>
            void Deactivate();
            /// <summary>
            /// Preprocesses WM_KEYUP window message.
            /// </summary>
            /// <param name="m">the Message object to process.</param>
            /// <returns>True, if the message was processed.</returns>
            Boolean PreProcessWmKeyUp(ref Message m);
            /// <summary>
            /// Preprocesses WM_CHAR window message.
            /// </summary>
            /// <param name="m">the Message object to process.</param>
            /// <returns>True, if the message was processed.</returns>
            Boolean PreProcessWmChar(ref Message m);
            /// <summary>
            /// Preprocesses WM_KEYDOWN window message.
            /// </summary>
            /// <param name="m">the Message object to process.</param>
            /// <returns>True, if the message was processed.</returns>
            Boolean PreProcessWmKeyDown(ref Message m);
            /// <summary>
            /// Gives some information about where to place the caret.
            /// </summary>
            /// <param name="byteIndex">the index of the byte</param>
            /// <returns>the position where the caret is to place.</returns>
            PointF GetCaretPointF(Int64 byteIndex);
        }
        #endregion
        #region EmptyKeyInterpreter class
        /// <summary>
        /// Represents an empty input handler without any functionality. 
        /// If is set ByteProvider to null, then this interpreter is used.
        /// </summary>
        class EmptyKeyInterpreter : IKeyInterpreter
        {
            HexBox _hexBox;

            public EmptyKeyInterpreter(HexBox hexBox)
            {
                _hexBox = hexBox;
            }

            #region IKeyInterpreter Members
            public void Activate() { }
            public void Deactivate() { }

            public Boolean PreProcessWmKeyUp(ref Message m)
            { return _hexBox.BasePreProcessMessage(ref m); }

            public Boolean PreProcessWmChar(ref Message m)
            { return _hexBox.BasePreProcessMessage(ref m); }

            public Boolean PreProcessWmKeyDown(ref Message m)
            { return _hexBox.BasePreProcessMessage(ref m); }

            public PointF GetCaretPointF(Int64 byteIndex)
            { return new PointF(); }

            #endregion
        }
        #endregion
        #region KeyInterpreter class
        /// <summary>
        /// Handles user input such as mouse and keyboard input during hex view edit
        /// </summary>
        class KeyInterpreter : IKeyInterpreter
        {
            /// <summary>
            /// Delegate for key-down processing.
            /// </summary>
            /// <param name="m">the message object contains key data information</param>
            /// <returns>True, if the message was processed</returns>
            delegate Boolean MessageDelegate(ref Message m);

            #region Fields
            /// <summary>
            /// Contains the parent HexBox control
            /// </summary>
            protected HexBox _hexBox;

            /// <summary>
            /// Contains True, if shift key is down
            /// </summary>
            protected Boolean _shiftDown;
            /// <summary>
            /// Contains True, if mouse is down
            /// </summary>
            Boolean _mouseDown;
            /// <summary>
            /// Contains the selection start position info
            /// </summary>
            BytePositionInfo _bpiStart;
            /// <summary>
            /// Contains the current mouse selection position info
            /// </summary>
            BytePositionInfo _bpi;
            /// <summary>
            /// Contains all message handlers of key interpreter key down message
            /// </summary>
            Dictionary<Keys, MessageDelegate> _messageHandlers;
            #endregion

            #region Ctors
            public KeyInterpreter(HexBox hexBox)
            {
                _hexBox = hexBox;
            }
            #endregion

            #region Activate, Deactive methods
            public virtual void Activate()
            {
                _hexBox.MouseDown += new MouseEventHandler(BeginMouseSelection);
                _hexBox.MouseMove += new MouseEventHandler(UpdateMouseSelection);
                _hexBox.MouseUp += new MouseEventHandler(EndMouseSelection);
            }

            public virtual void Deactivate()
            {
                _hexBox.MouseDown -= new MouseEventHandler(BeginMouseSelection);
                _hexBox.MouseMove -= new MouseEventHandler(UpdateMouseSelection);
                _hexBox.MouseUp -= new MouseEventHandler(EndMouseSelection);
            }
            #endregion

            #region Mouse selection methods
            void BeginMouseSelection(Object sender, MouseEventArgs e)
            {
                System.Diagnostics.Debug.WriteLine("BeginMouseSelection()", "KeyInterpreter");

                if (e.Button != MouseButtons.Left)
                    return;

                _mouseDown = true;

                if (!_shiftDown)
                {
                    _bpiStart = new BytePositionInfo(_hexBox._bytePos, _hexBox._byteCharacterPos);
                    _hexBox.ReleaseSelection();
                }
                else
                {
                    UpdateMouseSelection(this, e);
                }
            }

            void UpdateMouseSelection(Object sender, MouseEventArgs e)
            {
                if (!_mouseDown)
                    return;

                _bpi = GetBytePositionInfo(new Point(e.X, e.Y));
                Int64 selEnd = _bpi.Index;
                Int64 realselStart;
                Int64 realselLength;

                if (selEnd < _bpiStart.Index)
                {
                    realselStart = selEnd;
                    realselLength = _bpiStart.Index - selEnd;
                }
                else if (selEnd > _bpiStart.Index)
                {
                    realselStart = _bpiStart.Index;
                    realselLength = selEnd - realselStart;
                }
                else
                {
                    realselStart = _hexBox._bytePos;
                    realselLength = 0;
                }

                if (realselStart != _hexBox._bytePos || realselLength != _hexBox._selectionLength)
                {
                    _hexBox.InternalSelect(realselStart, realselLength);
                    _hexBox.ScrollByteIntoView(_bpi.Index);
                }
            }

            void EndMouseSelection(Object sender, MouseEventArgs e)
            {
                _mouseDown = false;
            }
            #endregion

            #region PrePrcessWmKeyDown methods
            public virtual Boolean PreProcessWmKeyDown(ref Message m)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessWmKeyDown(ref Message m)", "KeyInterpreter");

                Keys vc = (Keys)m.WParam.ToInt32();

                Keys keyData = vc | Control.ModifierKeys;

                // detect whether key down event should be raised
                var hasMessageHandler = this.MessageHandlers.ContainsKey(keyData);
                if (hasMessageHandler && RaiseKeyDown(keyData))
                    return true;

                MessageDelegate messageHandler = hasMessageHandler
                    ? this.MessageHandlers[keyData]
                    : messageHandler = new MessageDelegate(PreProcessWmKeyDown_Default);

                return messageHandler(ref m);
            }

            protected Boolean PreProcessWmKeyDown_Default(ref Message m)
            {
                _hexBox.ScrollByteIntoView();
                return _hexBox.BasePreProcessMessage(ref m);
            }

            protected Boolean RaiseKeyDown(Keys keyData)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                _hexBox.OnKeyDown(e);
                return e.Handled;
            }

            protected virtual Boolean PreProcessWmKeyDown_Left(ref Message m)
            {
                return PerformPosMoveLeft();
            }

            protected virtual Boolean PreProcessWmKeyDown_Up(ref Message m)
            {
                Int64 pos = _hexBox._bytePos;
                Int32 cp = _hexBox._byteCharacterPos;

                if (!(pos == 0 && cp == 0))
                {
                    pos = Math.Max(-1, pos - _hexBox._iHexMaxHBytes);
                    if (pos == -1)
                        return true;

                    _hexBox.SetPosition(pos);

                    if (pos < _hexBox._startByte)
                    {
                        _hexBox.PerformScrollLineUp();
                    }

                    _hexBox.UpdateCaret();
                    _hexBox.Invalidate();
                }

                _hexBox.ScrollByteIntoView();
                _hexBox.ReleaseSelection();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_Right(ref Message m)
            {
                return PerformPosMoveRight();
            }

            protected virtual Boolean PreProcessWmKeyDown_Down(ref Message m)
            {
                Int64 pos = _hexBox._bytePos;
                Int32 cp = _hexBox._byteCharacterPos;

                if (pos == _hexBox._byteProvider.Length && cp == 0)
                    return true;

                pos = Math.Min(_hexBox._byteProvider.Length, pos + _hexBox._iHexMaxHBytes);

                if (pos == _hexBox._byteProvider.Length)
                    cp = 0;

                _hexBox.SetPosition(pos, cp);

                if (pos > _hexBox._endByte - 1)
                {
                    _hexBox.PerformScrollLineDown();
                }

                _hexBox.UpdateCaret();
                _hexBox.ScrollByteIntoView();
                _hexBox.ReleaseSelection();
                _hexBox.Invalidate();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_PageUp(ref Message m)
            {
                Int64 pos = _hexBox._bytePos;
                Int32 cp = _hexBox._byteCharacterPos;

                if (pos == 0 && cp == 0)
                    return true;

                pos = Math.Max(0, pos - _hexBox._iHexMaxBytes);
                if (pos == 0)
                    return true;

                _hexBox.SetPosition(pos);

                if (pos < _hexBox._startByte)
                {
                    _hexBox.PerformScrollPageUp();
                }

                _hexBox.ReleaseSelection();
                _hexBox.UpdateCaret();
                _hexBox.Invalidate();
                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_PageDown(ref Message m)
            {
                Int64 pos = _hexBox._bytePos;
                Int32 cp = _hexBox._byteCharacterPos;

                if (pos == _hexBox._byteProvider.Length && cp == 0)
                    return true;

                pos = Math.Min(_hexBox._byteProvider.Length, pos + _hexBox._iHexMaxBytes);

                if (pos == _hexBox._byteProvider.Length)
                    cp = 0;

                _hexBox.SetPosition(pos, cp);

                if (pos > _hexBox._endByte - 1)
                {
                    _hexBox.PerformScrollPageDown();
                }

                _hexBox.ReleaseSelection();
                _hexBox.UpdateCaret();
                _hexBox.Invalidate();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftLeft(ref Message m)
            {
                Int64 pos = _hexBox._bytePos;
                Int64 sel = _hexBox._selectionLength;

                if (pos + sel < 1)
                    return true;

                if (pos + sel <= _bpiStart.Index)
                {
                    if (pos == 0)
                        return true;

                    pos--;
                    sel++;
                }
                else
                {
                    sel = Math.Max(0, sel - 1);
                }

                _hexBox.ScrollByteIntoView();
                _hexBox.InternalSelect(pos, sel);

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftUp(ref Message m)
            {
                Int64 pos = _hexBox._bytePos;
                Int64 sel = _hexBox._selectionLength;

                if (pos - _hexBox._iHexMaxHBytes < 0 && pos <= _bpiStart.Index)
                    return true;

                if (_bpiStart.Index >= pos + sel)
                {
                    pos = pos - _hexBox._iHexMaxHBytes;
                    sel += _hexBox._iHexMaxHBytes;
                    _hexBox.InternalSelect(pos, sel);
                    _hexBox.ScrollByteIntoView();
                }
                else
                {
                    sel -= _hexBox._iHexMaxHBytes;
                    if (sel < 0)
                    {
                        pos = _bpiStart.Index + sel;
                        sel = -sel;
                        _hexBox.InternalSelect(pos, sel);
                        _hexBox.ScrollByteIntoView();
                    }
                    else
                    {
                        sel -= _hexBox._iHexMaxHBytes;
                        _hexBox.InternalSelect(pos, sel);
                        _hexBox.ScrollByteIntoView(pos + sel);
                    }
                }

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftRight(ref Message m)
            {
                Int64 pos = _hexBox._bytePos;
                Int64 sel = _hexBox._selectionLength;

                if (pos + sel >= _hexBox._byteProvider.Length)
                    return true;

                if (_bpiStart.Index <= pos)
                {
                    sel++;
                    _hexBox.InternalSelect(pos, sel);
                    _hexBox.ScrollByteIntoView(pos + sel);
                }
                else
                {
                    pos++;
                    sel = Math.Max(0, sel - 1);
                    _hexBox.InternalSelect(pos, sel);
                    _hexBox.ScrollByteIntoView();
                }

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftDown(ref Message m)
            {
                Int64 pos = _hexBox._bytePos;
                Int64 sel = _hexBox._selectionLength;

                Int64 max = _hexBox._byteProvider.Length;

                if (pos + sel + _hexBox._iHexMaxHBytes > max)
                    return true;

                if (_bpiStart.Index <= pos)
                {
                    sel += _hexBox._iHexMaxHBytes;
                    _hexBox.InternalSelect(pos, sel);
                    _hexBox.ScrollByteIntoView(pos + sel);
                }
                else
                {
                    sel -= _hexBox._iHexMaxHBytes;
                    if (sel < 0)
                    {
                        pos = _bpiStart.Index;
                        sel = -sel;
                    }
                    else
                    {
                        pos += _hexBox._iHexMaxHBytes;
                        //sel -= _hexBox._iHexMaxHBytes;
                    }

                    _hexBox.InternalSelect(pos, sel);
                    _hexBox.ScrollByteIntoView();
                }

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_Tab(ref Message m)
            {
                if (_hexBox._stringViewVisible && _hexBox._keyInterpreter.GetType() == typeof(KeyInterpreter))
                {
                    _hexBox.ActivateStringKeyInterpreter();
                    _hexBox.ScrollByteIntoView();
                    _hexBox.ReleaseSelection();
                    _hexBox.UpdateCaret();
                    _hexBox.Invalidate();
                    return true;
                }

                if (_hexBox.Parent == null) return true;
                _hexBox.Parent.SelectNextControl(_hexBox, true, true, true, true);
                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftTab(ref Message m)
            {
                if (_hexBox._keyInterpreter is StringKeyInterpreter)
                {
                    _shiftDown = false;
                    _hexBox.ActivateKeyInterpreter();
                    _hexBox.ScrollByteIntoView();
                    _hexBox.ReleaseSelection();
                    _hexBox.UpdateCaret();
                    _hexBox.Invalidate();
                    return true;
                }

                if (_hexBox.Parent == null) return true;
                _hexBox.Parent.SelectNextControl(_hexBox, false, true, true, true);
                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_Back(ref Message m)
            {
                if (!_hexBox._byteProvider.SupportsDeleteBytes())
                    return true;

                if (_hexBox.ReadOnly)
                    return true;

                Int64 pos = _hexBox._bytePos;
                Int64 sel = _hexBox._selectionLength;
                Int32 cp = _hexBox._byteCharacterPos;

                Int64 startDelete = (cp == 0 && sel == 0) ? pos - 1 : pos;
                if (startDelete < 0 && sel < 1)
                    return true;

                Int64 bytesToDelete = (sel > 0) ? sel : 1;
                _hexBox._byteProvider.DeleteBytes(Math.Max(0, startDelete), bytesToDelete);
                _hexBox.UpdateScrollSize();

                if (sel == 0)
                    PerformPosMoveLeftByte();

                _hexBox.ReleaseSelection();
                _hexBox.Invalidate();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_Delete(ref Message m)
            {
                if (!_hexBox._byteProvider.SupportsDeleteBytes())
                    return true;

                if (_hexBox.ReadOnly)
                    return true;

                Int64 pos = _hexBox._bytePos;
                Int64 sel = _hexBox._selectionLength;

                if (pos >= _hexBox._byteProvider.Length)
                    return true;

                Int64 bytesToDelete = (sel > 0) ? sel : 1;
                _hexBox._byteProvider.DeleteBytes(pos, bytesToDelete);

                _hexBox.UpdateScrollSize();
                _hexBox.ReleaseSelection();
                _hexBox.Invalidate();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_Home(ref Message m)
            {
                Int64 pos = _hexBox._bytePos;
                Int32 cp = _hexBox._byteCharacterPos;

                if (pos < 1)
                    return true;

                pos = 0;
                cp = 0;
                _hexBox.SetPosition(pos, cp);

                _hexBox.ScrollByteIntoView();
                _hexBox.UpdateCaret();
                _hexBox.ReleaseSelection();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_End(ref Message m)
            {
                Int64 pos = _hexBox._bytePos;
                Int32 cp = _hexBox._byteCharacterPos;

                if (pos >= _hexBox._byteProvider.Length - 1)
                    return true;

                pos = _hexBox._byteProvider.Length;
                cp = 0;
                _hexBox.SetPosition(pos, cp);

                _hexBox.ScrollByteIntoView();
                _hexBox.UpdateCaret();
                _hexBox.ReleaseSelection();

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ShiftShiftKey(ref Message m)
            {
                if (_mouseDown)
                    return true;
                if (_shiftDown)
                    return true;

                _shiftDown = true;

                if (_hexBox._selectionLength > 0)
                    return true;

                _bpiStart = new BytePositionInfo(_hexBox._bytePos, _hexBox._byteCharacterPos);

                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ControlC(ref Message m)
            {
                _hexBox.Copy();
                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ControlX(ref Message m)
            {
                _hexBox.Cut();
                return true;
            }

            protected virtual Boolean PreProcessWmKeyDown_ControlV(ref Message m)
            {
                _hexBox.Paste();
                return true;
            }

            #endregion

            #region PreProcessWmChar methods
            public virtual Boolean PreProcessWmChar(ref Message m)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    return _hexBox.BasePreProcessMessage(ref m);
                }

                Boolean sw = _hexBox._byteProvider.SupportsWriteByte();
                Boolean si = _hexBox._byteProvider.SupportsInsertBytes();
                Boolean sd = _hexBox._byteProvider.SupportsDeleteBytes();

                Int64 pos = _hexBox._bytePos;
                Int64 sel = _hexBox._selectionLength;
                Int32 cp = _hexBox._byteCharacterPos;

                if (
                    (!sw && pos != _hexBox._byteProvider.Length) ||
                    (!si && pos == _hexBox._byteProvider.Length))
                {
                    return _hexBox.BasePreProcessMessage(ref m);
                }

                Char c = (Char)m.WParam.ToInt32();

                if (Uri.IsHexDigit(c))
                {
                    if (RaiseKeyPress(c))
                        return true;

                    if (_hexBox.ReadOnly)
                        return true;

                    Boolean isInsertMode = (pos == _hexBox._byteProvider.Length);

                    // do insert when insertActive = true
                    if (!isInsertMode && si && _hexBox.InsertActive && cp == 0)
                        isInsertMode = true;

                    if (sd && si && sel > 0)
                    {
                        _hexBox._byteProvider.DeleteBytes(pos, sel);
                        isInsertMode = true;
                        cp = 0;
                        _hexBox.SetPosition(pos, cp);
                    }

                    _hexBox.ReleaseSelection();

                    Byte currentByte;
                    if (isInsertMode)
                        currentByte = 0;
                    else
                        currentByte = _hexBox._byteProvider.ReadByte(pos);

                    String sCb = currentByte.ToString("X", System.Threading.Thread.CurrentThread.CurrentCulture);
                    if (sCb.Length == 1)
                        sCb = "0" + sCb;

                    String sNewCb = c.ToString();
                    if (cp == 0)
                        sNewCb += sCb.Substring(1, 1);
                    else
                        sNewCb = sCb.Substring(0, 1) + sNewCb;
                    Byte newcb = Byte.Parse(sNewCb, System.Globalization.NumberStyles.AllowHexSpecifier, System.Threading.Thread.CurrentThread.CurrentCulture);

                    if (isInsertMode)
                        _hexBox._byteProvider.InsertBytes(pos, new Byte[] { newcb });
                    else
                        _hexBox._byteProvider.WriteByte(pos, newcb);

                    PerformPosMoveRight();

                    _hexBox.Invalidate();
                    return true;
                }
                else
                {
                    return _hexBox.BasePreProcessMessage(ref m);
                }
            }

            protected Boolean RaiseKeyPress(Char keyChar)
            {
                KeyPressEventArgs e = new KeyPressEventArgs(keyChar);
                _hexBox.OnKeyPress(e);
                return e.Handled;
            }
            #endregion

            #region PreProcessWmKeyUp methods
            public virtual Boolean PreProcessWmKeyUp(ref Message m)
            {
                System.Diagnostics.Debug.WriteLine("PreProcessWmKeyUp(ref Message m)", "KeyInterpreter");

                Keys vc = (Keys)m.WParam.ToInt32();

                Keys keyData = vc | Control.ModifierKeys;

                switch (keyData)
                {
                    case Keys.ShiftKey:
                    case Keys.Insert:
                        if (RaiseKeyUp(keyData))
                            return true;
                        break;
                }

                switch (keyData)
                {
                    case Keys.ShiftKey:
                        _shiftDown = false;
                        return true;
                    case Keys.Insert:
                        return PreProcessWmKeyUp_Insert(ref m);
                    default:
                        return _hexBox.BasePreProcessMessage(ref m);
                }
            }

            protected virtual Boolean PreProcessWmKeyUp_Insert(ref Message m)
            {
                _hexBox.InsertActive = !_hexBox.InsertActive;
                return true;
            }

            protected Boolean RaiseKeyUp(Keys keyData)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                _hexBox.OnKeyUp(e);
                return e.Handled;
            }
            #endregion

            #region Misc
            Dictionary<Keys, MessageDelegate> MessageHandlers
            {
                get
                {
                    if (_messageHandlers == null)
                    {
                        _messageHandlers = new Dictionary<Keys, MessageDelegate>();
                        _messageHandlers.Add(Keys.Left, new MessageDelegate(PreProcessWmKeyDown_Left)); // move left
                        _messageHandlers.Add(Keys.Up, new MessageDelegate(PreProcessWmKeyDown_Up)); // move up
                        _messageHandlers.Add(Keys.Right, new MessageDelegate(PreProcessWmKeyDown_Right)); // move right
                        _messageHandlers.Add(Keys.Down, new MessageDelegate(PreProcessWmKeyDown_Down)); // move down
                        _messageHandlers.Add(Keys.PageUp, new MessageDelegate(PreProcessWmKeyDown_PageUp)); // move pageup
                        _messageHandlers.Add(Keys.PageDown, new MessageDelegate(PreProcessWmKeyDown_PageDown)); // move page down
                        _messageHandlers.Add(Keys.Left | Keys.Shift, new MessageDelegate(PreProcessWmKeyDown_ShiftLeft)); // move left with selection
                        _messageHandlers.Add(Keys.Up | Keys.Shift, new MessageDelegate(PreProcessWmKeyDown_ShiftUp)); // move up with selection
                        _messageHandlers.Add(Keys.Right | Keys.Shift, new MessageDelegate(PreProcessWmKeyDown_ShiftRight)); // move right with selection
                        _messageHandlers.Add(Keys.Down | Keys.Shift, new MessageDelegate(PreProcessWmKeyDown_ShiftDown)); // move down with selection
                        _messageHandlers.Add(Keys.Tab, new MessageDelegate(PreProcessWmKeyDown_Tab)); // switch to string view
                        _messageHandlers.Add(Keys.Back, new MessageDelegate(PreProcessWmKeyDown_Back)); // back
                        _messageHandlers.Add(Keys.Delete, new MessageDelegate(PreProcessWmKeyDown_Delete)); // delete
                        _messageHandlers.Add(Keys.Home, new MessageDelegate(PreProcessWmKeyDown_Home)); // move to home
                        _messageHandlers.Add(Keys.End, new MessageDelegate(PreProcessWmKeyDown_End)); // move to end
                        _messageHandlers.Add(Keys.ShiftKey | Keys.Shift, new MessageDelegate(PreProcessWmKeyDown_ShiftShiftKey)); // begin selection process
                        _messageHandlers.Add(Keys.C | Keys.Control, new MessageDelegate(PreProcessWmKeyDown_ControlC)); // copy 
                        _messageHandlers.Add(Keys.X | Keys.Control, new MessageDelegate(PreProcessWmKeyDown_ControlX)); // cut
                        _messageHandlers.Add(Keys.V | Keys.Control, new MessageDelegate(PreProcessWmKeyDown_ControlV)); // paste
                    }
                    return _messageHandlers;
                }
            }

            protected virtual Boolean PerformPosMoveLeft()
            {
                Int64 pos = _hexBox._bytePos;
                Int64 sel = _hexBox._selectionLength;
                Int32 cp = _hexBox._byteCharacterPos;

                if (sel != 0)
                {
                    cp = 0;
                    _hexBox.SetPosition(pos, cp);
                    _hexBox.ReleaseSelection();
                }
                else
                {
                    if (pos == 0 && cp == 0)
                        return true;

                    if (cp > 0)
                    {
                        cp--;
                    }
                    else
                    {
                        pos = Math.Max(0, pos - 1);
                        cp++;
                    }

                    _hexBox.SetPosition(pos, cp);

                    if (pos < _hexBox._startByte)
                    {
                        _hexBox.PerformScrollLineUp();
                    }
                    _hexBox.UpdateCaret();
                    _hexBox.Invalidate();
                }

                _hexBox.ScrollByteIntoView();
                return true;
            }
            protected virtual Boolean PerformPosMoveRight()
            {
                Int64 pos = _hexBox._bytePos;
                Int32 cp = _hexBox._byteCharacterPos;
                Int64 sel = _hexBox._selectionLength;

                if (sel != 0)
                {
                    pos += sel;
                    cp = 0;
                    _hexBox.SetPosition(pos, cp);
                    _hexBox.ReleaseSelection();
                }
                else
                {
                    if (!(pos == _hexBox._byteProvider.Length && cp == 0))
                    {

                        if (cp > 0)
                        {
                            pos = Math.Min(_hexBox._byteProvider.Length, pos + 1);
                            cp = 0;
                        }
                        else
                        {
                            cp++;
                        }

                        _hexBox.SetPosition(pos, cp);

                        if (pos > _hexBox._endByte - 1)
                        {
                            _hexBox.PerformScrollLineDown();
                        }
                        _hexBox.UpdateCaret();
                        _hexBox.Invalidate();
                    }
                }

                _hexBox.ScrollByteIntoView();
                return true;
            }
            protected virtual Boolean PerformPosMoveLeftByte()
            {
                Int64 pos = _hexBox._bytePos;
                Int32 cp = _hexBox._byteCharacterPos;

                if (pos == 0)
                    return true;

                pos = Math.Max(0, pos - 1);
                cp = 0;

                _hexBox.SetPosition(pos, cp);

                if (pos < _hexBox._startByte)
                {
                    _hexBox.PerformScrollLineUp();
                }
                _hexBox.UpdateCaret();
                _hexBox.ScrollByteIntoView();
                _hexBox.Invalidate();

                return true;
            }

            protected virtual Boolean PerformPosMoveRightByte()
            {
                Int64 pos = _hexBox._bytePos;
                Int32 cp = _hexBox._byteCharacterPos;

                if (pos == _hexBox._byteProvider.Length)
                    return true;

                pos = Math.Min(_hexBox._byteProvider.Length, pos + 1);
                cp = 0;

                _hexBox.SetPosition(pos, cp);

                if (pos > _hexBox._endByte - 1)
                {
                    _hexBox.PerformScrollLineDown();
                }
                _hexBox.UpdateCaret();
                _hexBox.ScrollByteIntoView();
                _hexBox.Invalidate();

                return true;
            }


            public virtual PointF GetCaretPointF(Int64 byteIndex)
            {
                System.Diagnostics.Debug.WriteLine("GetCaretPointF()", "KeyInterpreter");

                return _hexBox.GetBytePointF(byteIndex);
            }

            protected virtual BytePositionInfo GetBytePositionInfo(Point p)
            {
                return _hexBox.GetHexBytePositionInfo(p);
            }
            #endregion
        }
        #endregion
        #region StringKeyInterpreter class
        /// <summary>
        /// Handles user input such as mouse and keyboard input during string view edit
        /// </summary>
        class StringKeyInterpreter : KeyInterpreter
        {
            #region Ctors
            public StringKeyInterpreter(HexBox hexBox)
                : base(hexBox)
            {
                _hexBox._byteCharacterPos = 0;
            }
            #endregion

            #region PreProcessWmKeyDown methods
            public override Boolean PreProcessWmKeyDown(ref Message m)
            {
                Keys vc = (Keys)m.WParam.ToInt32();

                Keys keyData = vc | Control.ModifierKeys;

                switch (keyData)
                {
                    case Keys.Tab | Keys.Shift:
                    case Keys.Tab:
                        if (RaiseKeyDown(keyData))
                            return true;
                        break;
                }

                switch (keyData)
                {
                    case Keys.Tab | Keys.Shift:
                        return PreProcessWmKeyDown_ShiftTab(ref m);
                    case Keys.Tab:
                        return PreProcessWmKeyDown_Tab(ref m);
                    default:
                        return base.PreProcessWmKeyDown(ref m);
                }
            }

            protected override Boolean PreProcessWmKeyDown_Left(ref Message m)
            {
                return PerformPosMoveLeftByte();
            }

            protected override Boolean PreProcessWmKeyDown_Right(ref Message m)
            {
                return PerformPosMoveRightByte();
            }

            #endregion

            #region PreProcessWmChar methods
            public override Boolean PreProcessWmChar(ref Message m)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    return _hexBox.BasePreProcessMessage(ref m);
                }

                Boolean sw = _hexBox._byteProvider.SupportsWriteByte();
                Boolean si = _hexBox._byteProvider.SupportsInsertBytes();
                Boolean sd = _hexBox._byteProvider.SupportsDeleteBytes();

                Int64 pos = _hexBox._bytePos;
                Int64 sel = _hexBox._selectionLength;
                Int32 cp = _hexBox._byteCharacterPos;

                if (
                    (!sw && pos != _hexBox._byteProvider.Length) ||
                    (!si && pos == _hexBox._byteProvider.Length))
                {
                    return _hexBox.BasePreProcessMessage(ref m);
                }

                Char c = (Char)m.WParam.ToInt32();

                if (RaiseKeyPress(c))
                    return true;

                if (_hexBox.ReadOnly)
                    return true;

                Boolean isInsertMode = (pos == _hexBox._byteProvider.Length);

                // do insert when insertActive = true
                if (!isInsertMode && si && _hexBox.InsertActive)
                    isInsertMode = true;

                if (sd && si && sel > 0)
                {
                    _hexBox._byteProvider.DeleteBytes(pos, sel);
                    isInsertMode = true;
                    cp = 0;
                    _hexBox.SetPosition(pos, cp);
                }

                _hexBox.ReleaseSelection();

                Byte b = _hexBox.ByteCharConverter.ToByte(c);
                if (isInsertMode)
                    _hexBox._byteProvider.InsertBytes(pos, new Byte[] { b });
                else
                    _hexBox._byteProvider.WriteByte(pos, b);

                PerformPosMoveRightByte();
                _hexBox.Invalidate();

                return true;
            }
            #endregion

            #region Misc
            public override PointF GetCaretPointF(Int64 byteIndex)
            {
                System.Diagnostics.Debug.WriteLine("GetCaretPointF()", "StringKeyInterpreter");

                Point gp = _hexBox.GetGridBytePoint(byteIndex);
                return _hexBox.GetByteStringPointF(gp);
            }

            protected override BytePositionInfo GetBytePositionInfo(Point p)
            {
                return _hexBox.GetStringBytePositionInfo(p);
            }
            #endregion
        }
        #endregion



		#region Paint methods
		/// <summary>
		/// Paints the background.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			switch (_borderStyle)
			{
				case BorderStyle.Fixed3D:
					{
						if (TextBoxRenderer.IsSupported)
						{
							VisualStyleElement state = VisualStyleElement.TextBox.TextEdit.Normal;
							Color backColor = this.BackColor;

							if (this.Enabled)
							{
								if (this.ReadOnly)
									state = VisualStyleElement.TextBox.TextEdit.ReadOnly;
								else if (this.Focused)
									state = VisualStyleElement.TextBox.TextEdit.Focused;
							}
							else
							{
								state = VisualStyleElement.TextBox.TextEdit.Disabled;
								backColor = this.BackColorDisabled;
							}

							VisualStyleRenderer vsr = new VisualStyleRenderer(state);
							vsr.DrawBackground(e.Graphics, this.ClientRectangle);

							Rectangle rectContent = vsr.GetBackgroundContentRectangle(e.Graphics, this.ClientRectangle);
							e.Graphics.FillRectangle(new SolidBrush(backColor), rectContent);
						}
						else
						{
							// draw background
							e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

							// draw default border
							ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Sunken);
						}

						break;
					}
				case BorderStyle.FixedSingle:
					{
						// draw background
						e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

						// draw fixed single border
						ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
						break;
					}
				default:
					{
						// draw background
						e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
						break;
					}
			}
		}


		/// <summary>
		/// Paints the hex box.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (_byteProvider == null)
				return;

			System.Diagnostics.Debug.WriteLine("OnPaint " + DateTime.Now.ToString(), "HexBox");

			// draw only in the content rectangle, so exclude the border and the scrollbar.
			Region r = new Region(ClientRectangle);
			r.Exclude(_recContent);
			e.Graphics.ExcludeClip(r);

			UpdateVisibilityBytes();


			if (_lineInfoVisible)
				PaintLineInfo(e.Graphics, _startByte, _endByte);

			if (!_stringViewVisible)
			{
				PaintHex(e.Graphics, _startByte, _endByte);
			}
			else
			{
				PaintHexAndStringView(e.Graphics, _startByte, _endByte);
				if (_shadowSelectionVisible)
					PaintCurrentBytesSign(e.Graphics);
			}
			if (_columnInfoVisible)
				PaintHeaderRow(e.Graphics);
			if (_groupSeparatorVisible)
				PaintColumnSeparator(e.Graphics);
		}


		void PaintLineInfo(Graphics g, Int64 startByte, Int64 endByte)
		{
			// Ensure endByte isn't > length of array.
			endByte = Math.Min(_byteProvider.Length - 1, endByte);

			Color lineInfoColor = (this.InfoForeColor != Color.Empty) ? this.InfoForeColor : this.ForeColor;
			Brush brush = new SolidBrush(lineInfoColor);

            Int32 maxLine = GetGridBytePoint(endByte - startByte).Y + 1;

			for (Int32 i = 0; i < maxLine; i++)
			{
                Int64 firstLineByte = (startByte + (_iHexMaxHBytes) * i) + _lineInfoOffset;

				PointF bytePointF = GetBytePointF(new Point(0, 0 + i));
                String info = firstLineByte.ToString(_hexStringFormat, System.Threading.Thread.CurrentThread.CurrentCulture);
                Int32 nulls = 8 - info.Length;
                String formattedInfo;
				if (nulls > -1)
				{
					formattedInfo = new String('0', 8 - info.Length) + info;
				}
				else
				{
					formattedInfo = new String('~', 8);
				}

				g.DrawString(formattedInfo, Font, brush, new PointF(_recLineInfo.X, bytePointF.Y), _stringFormat);
			}
		}

		void PaintHeaderRow(Graphics g)
		{
			Brush brush = new SolidBrush(this.InfoForeColor);
			for (Int32 col = 0; col < _iHexMaxHBytes; col++)
			{
				PaintColumnInfo(g, (Byte)col, brush, col);
			}
		}

		void PaintColumnSeparator(Graphics g)
		{
			for (Int32 col = GroupSize; col < _iHexMaxHBytes; col += GroupSize)
			{
				var pen = new Pen(new SolidBrush(this.InfoForeColor), 1);
				PointF headerPointF = GetColumnInfoPointF(col);
				headerPointF.X -= _charSize.Width / 2;
				g.DrawLine(pen, headerPointF, new PointF(headerPointF.X, headerPointF.Y + _recColumnInfo.Height + _recHex.Height));
				if (StringViewVisible)
				{
					PointF byteStringPointF = GetByteStringPointF(new Point(col, 0));
					headerPointF.X -= 2;
					g.DrawLine(pen, new PointF(byteStringPointF.X, byteStringPointF.Y), new PointF(byteStringPointF.X, byteStringPointF.Y + _recHex.Height));
				}
			}
		}

		void PaintHex(Graphics g, Int64 startByte, Int64 endByte)
		{
			Brush brush = new SolidBrush(GetDefaultForeColor());
			Brush selBrush = new SolidBrush(_selectionForeColor);
			Brush selBrushBack = new SolidBrush(_selectionBackColor);

            Int32 counter = -1;
            Int64 intern_endByte = Math.Min(_byteProvider.Length - 1, endByte + _iHexMaxHBytes);

            Boolean isKeyInterpreterActive = _keyInterpreter == null || _keyInterpreter.GetType() == typeof(KeyInterpreter);

			for (Int64 i = startByte; i < intern_endByte + 1; i++)
			{
				counter++;
				Point gridPoint = GetGridBytePoint(counter);
                Byte b = _byteProvider.ReadByte(i);

                Boolean isSelectedByte = i >= _bytePos && i <= (_bytePos + _selectionLength - 1) && _selectionLength != 0;

				if (isSelectedByte && isKeyInterpreterActive)
				{
					PaintHexStringSelected(g, b, selBrush, selBrushBack, gridPoint);
				}
				else
				{
					PaintHexString(g, b, brush, gridPoint);
				}
			}
		}

		void PaintHexString(Graphics g, Byte b, Brush brush, Point gridPoint)
		{
			PointF bytePointF = GetBytePointF(gridPoint);

            String sB = ConvertByteToHex(b);

			g.DrawString(sB.Substring(0, 1), Font, brush, bytePointF, _stringFormat);
			bytePointF.X += _charSize.Width;
			g.DrawString(sB.Substring(1, 1), Font, brush, bytePointF, _stringFormat);
		}

		void PaintColumnInfo(Graphics g, Byte b, Brush brush, Int32 col)
		{
			PointF headerPointF = GetColumnInfoPointF(col);

            String sB = ConvertByteToHex(b);

			g.DrawString(sB.Substring(0, 1), Font, brush, headerPointF, _stringFormat);
			headerPointF.X += _charSize.Width;
			g.DrawString(sB.Substring(1, 1), Font, brush, headerPointF, _stringFormat);
		}

		void PaintHexStringSelected(Graphics g, Byte b, Brush brush, Brush brushBack, Point gridPoint)
		{
            String sB = b.ToString(_hexStringFormat, System.Threading.Thread.CurrentThread.CurrentCulture);
			if (sB.Length == 1)
				sB = "0" + sB;

			PointF bytePointF = GetBytePointF(gridPoint);

            Boolean isLastLineChar = (gridPoint.X + 1 == _iHexMaxHBytes);
            Single bcWidth = (isLastLineChar) ? _charSize.Width * 2 : _charSize.Width * 3;

			g.FillRectangle(brushBack, bytePointF.X, bytePointF.Y, bcWidth, _charSize.Height);
			g.DrawString(sB.Substring(0, 1), Font, brush, bytePointF, _stringFormat);
			bytePointF.X += _charSize.Width;
			g.DrawString(sB.Substring(1, 1), Font, brush, bytePointF, _stringFormat);
		}

		void PaintHexAndStringView(Graphics g, Int64 startByte, Int64 endByte)
		{
			Brush brush = new SolidBrush(GetDefaultForeColor());
			Brush selBrush = new SolidBrush(_selectionForeColor);
			Brush selBrushBack = new SolidBrush(_selectionBackColor);

            Int32 counter = -1;
            Int64 intern_endByte = Math.Min(_byteProvider.Length - 1, endByte + _iHexMaxHBytes);

            Boolean isKeyInterpreterActive = _keyInterpreter == null || _keyInterpreter.GetType() == typeof(KeyInterpreter);
            Boolean isStringKeyInterpreterActive = _keyInterpreter != null && _keyInterpreter.GetType() == typeof(StringKeyInterpreter);

			for (Int64 i = startByte; i < intern_endByte + 1; i++)
			{
				counter++;
				Point gridPoint = GetGridBytePoint(counter);
				PointF byteStringPointF = GetByteStringPointF(gridPoint);
                Byte b = _byteProvider.ReadByte(i);

                Boolean isSelectedByte = i >= _bytePos && i <= (_bytePos + _selectionLength - 1) && _selectionLength != 0;

				if (isSelectedByte && isKeyInterpreterActive)
				{
					PaintHexStringSelected(g, b, selBrush, selBrushBack, gridPoint);
				}
				else
				{
					PaintHexString(g, b, brush, gridPoint);
				}

                String s = new String(ByteCharConverter.ToChar(b), 1);

				if (isSelectedByte && isStringKeyInterpreterActive)
				{
					g.FillRectangle(selBrushBack, byteStringPointF.X, byteStringPointF.Y, _charSize.Width, _charSize.Height);
					g.DrawString(s, Font, selBrush, byteStringPointF, _stringFormat);
				}
				else
				{
					g.DrawString(s, Font, brush, byteStringPointF, _stringFormat);
				}
			}
		}

		void PaintCurrentBytesSign(Graphics g)
		{
			if (_keyInterpreter != null && _bytePos != -1 && Enabled)
			{
				if (_keyInterpreter.GetType() == typeof(KeyInterpreter))
				{
					if (_selectionLength == 0)
					{
						Point gp = GetGridBytePoint(_bytePos - _startByte);
						PointF pf = GetByteStringPointF(gp);
						Size s = new Size((Int32)_charSize.Width, (Int32)_charSize.Height);
						Rectangle r = new Rectangle((Int32)pf.X, (Int32)pf.Y, s.Width, s.Height);
						if (r.IntersectsWith(_recStringView))
						{
							r.Intersect(_recStringView);
							PaintCurrentByteSign(g, r);
						}
					}
					else
					{
                        Int32 lineWidth = (Int32)(_recStringView.Width - _charSize.Width);

						Point startSelGridPoint = GetGridBytePoint(_bytePos - _startByte);
						PointF startSelPointF = GetByteStringPointF(startSelGridPoint);

						Point endSelGridPoint = GetGridBytePoint(_bytePos - _startByte + _selectionLength - 1);
						PointF endSelPointF = GetByteStringPointF(endSelGridPoint);

                        Int32 multiLine = endSelGridPoint.Y - startSelGridPoint.Y;
						if (multiLine == 0)
						{
							
							Rectangle singleLine = new Rectangle(
								(Int32)startSelPointF.X,
								(Int32)startSelPointF.Y,
								(Int32)(endSelPointF.X - startSelPointF.X + _charSize.Width),
								(Int32)_charSize.Height);
							if (singleLine.IntersectsWith(_recStringView))
							{
								singleLine.Intersect(_recStringView);
								PaintCurrentByteSign(g, singleLine);
							}
						}
						else
						{
							Rectangle firstLine = new Rectangle(
								(Int32)startSelPointF.X,
								(Int32)startSelPointF.Y,
								(Int32)(_recStringView.X + lineWidth - startSelPointF.X + _charSize.Width),
								(Int32)_charSize.Height);
							if (firstLine.IntersectsWith(_recStringView))
							{
								firstLine.Intersect(_recStringView);
								PaintCurrentByteSign(g, firstLine);
							}

							if (multiLine > 1)
							{
								Rectangle betweenLines = new Rectangle(
									_recStringView.X,
									(Int32)(startSelPointF.Y + _charSize.Height),
									(Int32)(_recStringView.Width),
									(Int32)(_charSize.Height * (multiLine - 1)));
								if (betweenLines.IntersectsWith(_recStringView))
								{
									betweenLines.Intersect(_recStringView);
									PaintCurrentByteSign(g, betweenLines);
								}

							}

							Rectangle lastLine = new Rectangle(
								_recStringView.X,
								(Int32)endSelPointF.Y,
								(Int32)(endSelPointF.X - _recStringView.X + _charSize.Width),
								(Int32)_charSize.Height);
							if (lastLine.IntersectsWith(_recStringView))
							{
								lastLine.Intersect(_recStringView);
								PaintCurrentByteSign(g, lastLine);
							}
						}
					}
				}
				else
				{
					if (_selectionLength == 0)
					{
						Point gp = GetGridBytePoint(_bytePos - _startByte);
						PointF pf = GetBytePointF(gp);
						Size s = new Size((Int32)_charSize.Width * 2, (Int32)_charSize.Height);
						Rectangle r = new Rectangle((Int32)pf.X, (Int32)pf.Y, s.Width, s.Height);
						PaintCurrentByteSign(g, r);
					}
					else
					{
                        Int32 lineWidth = (Int32)(_recHex.Width - _charSize.Width * 5);

						Point startSelGridPoint = GetGridBytePoint(_bytePos - _startByte);
						PointF startSelPointF = GetBytePointF(startSelGridPoint);

						Point endSelGridPoint = GetGridBytePoint(_bytePos - _startByte + _selectionLength - 1);
						PointF endSelPointF = GetBytePointF(endSelGridPoint);

                        Int32 multiLine = endSelGridPoint.Y - startSelGridPoint.Y;
						if (multiLine == 0)
						{
							Rectangle singleLine = new Rectangle(
								(Int32)startSelPointF.X,
								(Int32)startSelPointF.Y,
								(Int32)(endSelPointF.X - startSelPointF.X + _charSize.Width * 2),
								(Int32)_charSize.Height);
							if (singleLine.IntersectsWith(_recHex))
							{
								singleLine.Intersect(_recHex);
								PaintCurrentByteSign(g, singleLine);
							}
						}
						else
						{
							Rectangle firstLine = new Rectangle(
								(Int32)startSelPointF.X,
								(Int32)startSelPointF.Y,
								(Int32)(_recHex.X + lineWidth - startSelPointF.X + _charSize.Width * 2),
								(Int32)_charSize.Height);
							if (firstLine.IntersectsWith(_recHex))
							{
								firstLine.Intersect(_recHex);
								PaintCurrentByteSign(g, firstLine);
							}

							if (multiLine > 1)
							{
								Rectangle betweenLines = new Rectangle(
									_recHex.X,
									(Int32)(startSelPointF.Y + _charSize.Height),
									(Int32)(lineWidth + _charSize.Width * 2),
									(Int32)(_charSize.Height * (multiLine - 1)));
								if (betweenLines.IntersectsWith(_recHex))
								{
									betweenLines.Intersect(_recHex);
									PaintCurrentByteSign(g, betweenLines);
								}

							}

							Rectangle lastLine = new Rectangle(
								_recHex.X,
								(Int32)endSelPointF.Y,
								(Int32)(endSelPointF.X - _recHex.X + _charSize.Width * 2),
								(Int32)_charSize.Height);
							if (lastLine.IntersectsWith(_recHex))
							{
								lastLine.Intersect(_recHex);
								PaintCurrentByteSign(g, lastLine);
							}
						}
					}
				}
			}
		}

		void PaintCurrentByteSign(Graphics g, Rectangle rec)
		{
			// stack overflowexception on big files - workaround
			if (rec.Top < 0 || rec.Left < 0 || rec.Width <= 0 || rec.Height <= 0)
				return;

			Bitmap myBitmap = new Bitmap(rec.Width, rec.Height);
			Graphics bitmapGraphics = Graphics.FromImage(myBitmap);

			SolidBrush greenBrush = new SolidBrush(_shadowSelectionColor);

			bitmapGraphics.FillRectangle(greenBrush, 0,
				0, rec.Width, rec.Height);

			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;

			g.DrawImage(myBitmap, rec.Left, rec.Top);
		}

		Color GetDefaultForeColor()
		{
			if (Enabled)
				return ForeColor;
			else
				return Color.Gray;
		}
		void UpdateVisibilityBytes()
		{
			if (_byteProvider == null || _byteProvider.Length == 0)
				return;

			_startByte = (_scrollVpos + 1) * _iHexMaxHBytes - _iHexMaxHBytes;
			_endByte = (Int64)Math.Min(_byteProvider.Length - 1, _startByte + _iHexMaxBytes);
		}
		#endregion

		#region Positioning methods
		void UpdateRectanglePositioning()
		{
			// calc char size
            SizeF charSize;
            using (var graphics = this.CreateGraphics())
            {
                charSize = this.CreateGraphics().MeasureString("A", Font, 100, _stringFormat);
            }
			CharSize = new SizeF((Single)Math.Ceiling(charSize.Width), (Single)Math.Ceiling(charSize.Height));

            Int32 requiredWidth = 0;

			// calc content bounds
			_recContent = ClientRectangle;
			_recContent.X += _recBorderLeft;
			_recContent.Y += _recBorderTop;
			_recContent.Width -= _recBorderRight + _recBorderLeft;
			_recContent.Height -= _recBorderBottom + _recBorderTop;

			if (_vScrollBarVisible)
			{
				_recContent.Width -= _vScrollBar.Width;
				_vScrollBar.Left = _recContent.X + _recContent.Width;
				_vScrollBar.Top = _recContent.Y;
				_vScrollBar.Height = _recContent.Height;
                requiredWidth += _vScrollBar.Width;
			}

            Int32 marginLeft = 4;

			// calc line info bounds
			if (_lineInfoVisible)
			{
				_recLineInfo = new Rectangle(_recContent.X + marginLeft,
					_recContent.Y,
					(Int32)(_charSize.Width * 10),
					_recContent.Height);
                requiredWidth += _recLineInfo.Width;
			}
			else
			{
				_recLineInfo = Rectangle.Empty;
				_recLineInfo.X = marginLeft;
                requiredWidth += marginLeft;
			}

			// calc line info bounds
			_recColumnInfo = new Rectangle(_recLineInfo.X + _recLineInfo.Width, _recContent.Y, _recContent.Width - _recLineInfo.Width, (Int32)charSize.Height + 4);
			if (_columnInfoVisible)
			{
				_recLineInfo.Y += (Int32)charSize.Height + 4;
				_recLineInfo.Height -= (Int32)charSize.Height + 4;
			}
			else
			{
				_recColumnInfo.Height = 0;
			}

			// calc hex bounds and grid
			_recHex = new Rectangle(_recLineInfo.X + _recLineInfo.Width,
				_recLineInfo.Y,
				_recContent.Width - _recLineInfo.Width,
				_recContent.Height - _recColumnInfo.Height);

			if (UseFixedBytesPerLine)
			{
				SetHorizontalByteCount(_bytesPerLine);
				_recHex.Width = (Int32)Math.Floor(((Double)_iHexMaxHBytes) * _charSize.Width * 3 + (2 * _charSize.Width));
                requiredWidth += _recHex.Width;
			}
			else
			{
                Int32 hmax = (Int32)Math.Floor((Double)_recHex.Width / (Double)_charSize.Width);
				if (_stringViewVisible)
				{
					hmax -= 2;
					if (hmax > 1)
						SetHorizontalByteCount((Int32)Math.Floor((Double)hmax / 4));
					else
						SetHorizontalByteCount(1);
				}
				else
				{
					if (hmax > 1)
						SetHorizontalByteCount((Int32)Math.Floor((Double)hmax / 3));
					else
						SetHorizontalByteCount(1);
				}
				_recHex.Width = (Int32)Math.Floor(((Double)_iHexMaxHBytes) * _charSize.Width * 3 + (2 * _charSize.Width));
                requiredWidth += _recHex.Width;
			}

			if (_stringViewVisible)
			{
				_recStringView = new Rectangle(_recHex.X + _recHex.Width,
					_recHex.Y,
					(Int32)(_charSize.Width * _iHexMaxHBytes),
					_recHex.Height);
                requiredWidth += _recStringView.Width;
			}
			else
			{
				_recStringView = Rectangle.Empty;
			}

            RequiredWidth = requiredWidth;

            Int32 vmax = (Int32)Math.Floor((Double)_recHex.Height / (Double)_charSize.Height);
			SetVerticalByteCount(vmax);

			_iHexMaxBytes = _iHexMaxHBytes * _iHexMaxVBytes;

			UpdateScrollSize();
		}

		PointF GetBytePointF(Int64 byteIndex)
		{
			Point gp = GetGridBytePoint(byteIndex);

			return GetBytePointF(gp);
		}

		PointF GetBytePointF(Point gp)
		{
            Single x = (3 * _charSize.Width) * gp.X + _recHex.X;
            Single y = (gp.Y + 1) * _charSize.Height - _charSize.Height + _recHex.Y;

			return new PointF(x, y);
		}
		PointF GetColumnInfoPointF(Int32 col)
		{
			Point gp = GetGridBytePoint(col);
            Single x = (3 * _charSize.Width) * gp.X + _recColumnInfo.X;
            Single y = _recColumnInfo.Y;

			return new PointF(x, y);
		}

		PointF GetByteStringPointF(Point gp)
		{
            Single x = (_charSize.Width) * gp.X + _recStringView.X;
            Single y = (gp.Y + 1) * _charSize.Height - _charSize.Height + _recStringView.Y;

			return new PointF(x, y);
		}

		Point GetGridBytePoint(Int64 byteIndex)
		{
            Int32 row = (Int32)Math.Floor((Double)byteIndex / (Double)_iHexMaxHBytes);
            Int32 column = (Int32)(byteIndex + _iHexMaxHBytes - _iHexMaxHBytes * (row + 1));

			Point res = new Point(column, row);
			return res;
		}
        #endregion

        #region Misc
        /// <summary>
        /// Converts a byte array to a hex string. For example: {10,11} = "0A 0B"
        /// </summary>
        /// <param name="data">the byte array</param>
        /// <returns>the hex string</returns>
        String ConvertBytesToHex(Byte[] data)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Byte b in data)
			{
                String hex = ConvertByteToHex(b);
				sb.Append(hex);
				sb.Append(" ");
			}
			if (sb.Length > 0)
				sb.Remove(sb.Length - 1, 1);
            String result = sb.ToString();
			return result;
		}
        /// <summary>
        /// Converts one byte to a hex string. For example: "10" = "0A";
        /// </summary>
        /// <param name="b">the byte to format</param>
        /// <returns>the hex string</returns>
        String ConvertByteToHex(Byte b)
		{
            String sB = b.ToString(_hexStringFormat, System.Threading.Thread.CurrentThread.CurrentCulture);
			if (sB.Length == 1)
				sB = "0" + sB;
			return sB;
		}
        /// <summary>
        /// Converts a hex string to a byte array. The hex string must be separated by a space char ' '. If there is any invalid hex information in the string the result will be null.
        /// </summary>
        /// <param name="hex">the hex string separated by ' '. For example: "0A 0B 0C"</param>
        /// <returns>the byte array. null if hex is invalid or empty</returns>
        Byte[] ConvertHexToBytes(String hex)
		{
			if (String.IsNullOrEmpty(hex))
				return null;
			hex = hex.Trim();
			var hexArray = hex.Split(' ');
			var byteArray = new Byte[hexArray.Length];

			for (Int32 i = 0; i < hexArray.Length; i++)
			{
				var hexValue = hexArray[i];

                Byte b;
				var isByte = ConvertHexToByte(hexValue, out b);
				if (!isByte)
					return null;
				byteArray[i] = b;
			}

			return byteArray;
		}

        Boolean ConvertHexToByte(String hex, out Byte b)
		{
            Boolean isByte = Byte.TryParse(hex, System.Globalization.NumberStyles.HexNumber, System.Threading.Thread.CurrentThread.CurrentCulture, out b);
			return isByte;
		}

		void SetPosition(Int64 bytePos)
		{
			SetPosition(bytePos, _byteCharacterPos);
		}

		void SetPosition(Int64 bytePos, Int32 byteCharacterPos)
		{
			if (_byteCharacterPos != byteCharacterPos)
			{
				_byteCharacterPos = byteCharacterPos;
			}

			if (bytePos != _bytePos)
			{
				_bytePos = bytePos;
				CheckCurrentLineChanged();
				CheckCurrentPositionInLineChanged();

				OnSelectionStartChanged(EventArgs.Empty);
			}
		}

		void SetSelectionLength(Int64 selectionLength)
		{
			if (selectionLength != _selectionLength)
			{
				_selectionLength = selectionLength;
				OnSelectionLengthChanged(EventArgs.Empty);
			}
		}

		void SetHorizontalByteCount(Int32 value)
		{
			if (_iHexMaxHBytes == value)
				return;

			_iHexMaxHBytes = value;
			OnHorizontalByteCountChanged(EventArgs.Empty);
		}

		void SetVerticalByteCount(Int32 value)
		{
			if (_iHexMaxVBytes == value)
				return;

			_iHexMaxVBytes = value;
			OnVerticalByteCountChanged(EventArgs.Empty);
		}

		void CheckCurrentLineChanged()
		{
            Int64 currentLine = (Int64)Math.Floor((Double)_bytePos / (Double)_iHexMaxHBytes) + 1;

			if (_byteProvider == null && _currentLine != 0)
			{
				_currentLine = 0;
				OnCurrentLineChanged(EventArgs.Empty);
			}
			else if (currentLine != _currentLine)
			{
				_currentLine = currentLine;
				OnCurrentLineChanged(EventArgs.Empty);
			}
		}

		void CheckCurrentPositionInLineChanged()
		{
			Point gb = GetGridBytePoint(_bytePos);
            Int32 currentPositionInLine = gb.X + 1;

			if (_byteProvider == null && _currentPositionInLine != 0)
			{
				_currentPositionInLine = 0;
				OnCurrentPositionInLineChanged(EventArgs.Empty);
			}
			else if (currentPositionInLine != _currentPositionInLine)
			{
				_currentPositionInLine = currentPositionInLine;
				OnCurrentPositionInLineChanged(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Raises the InsertActiveChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnInsertActiveChanged(EventArgs e)
		{
			if (InsertActiveChanged != null)
				InsertActiveChanged(this, e);
		}

		/// <summary>
		/// Raises the ReadOnlyChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			if (ReadOnlyChanged != null)
				ReadOnlyChanged(this, e);
		}

		/// <summary>
		/// Raises the ByteProviderChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnByteProviderChanged(EventArgs e)
		{
			if (ByteProviderChanged != null)
				ByteProviderChanged(this, e);
		}

		/// <summary>
		/// Raises the SelectionStartChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnSelectionStartChanged(EventArgs e)
		{
			if (SelectionStartChanged != null)
				SelectionStartChanged(this, e);
		}

		/// <summary>
		/// Raises the SelectionLengthChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnSelectionLengthChanged(EventArgs e)
		{
			if (SelectionLengthChanged != null)
				SelectionLengthChanged(this, e);
		}

		/// <summary>
		/// Raises the LineInfoVisibleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnLineInfoVisibleChanged(EventArgs e)
		{
			if (LineInfoVisibleChanged != null)
				LineInfoVisibleChanged(this, e);
		}

		/// <summary>
		/// Raises the OnColumnInfoVisibleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnColumnInfoVisibleChanged(EventArgs e)
		{
			if (ColumnInfoVisibleChanged != null)
				ColumnInfoVisibleChanged(this, e);
		}

		/// <summary>
		/// Raises the ColumnSeparatorVisibleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnGroupSeparatorVisibleChanged(EventArgs e)
		{
			if (GroupSeparatorVisibleChanged != null)
				GroupSeparatorVisibleChanged(this, e);
		}

		/// <summary>
		/// Raises the StringViewVisibleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnStringViewVisibleChanged(EventArgs e)
		{
			if (StringViewVisibleChanged != null)
				StringViewVisibleChanged(this, e);
		}

		/// <summary>
		/// Raises the BorderStyleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			if (BorderStyleChanged != null)
				BorderStyleChanged(this, e);
		}

		/// <summary>
		/// Raises the UseFixedBytesPerLineChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnUseFixedBytesPerLineChanged(EventArgs e)
		{
			if (UseFixedBytesPerLineChanged != null)
				UseFixedBytesPerLineChanged(this, e);
		}

		/// <summary>
		/// Raises the GroupSizeChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnGroupSizeChanged(EventArgs e)
		{
			if (GroupSizeChanged != null)
				GroupSizeChanged(this, e);
		}

		/// <summary>
		/// Raises the BytesPerLineChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnBytesPerLineChanged(EventArgs e)
		{
			if (BytesPerLineChanged != null)
				BytesPerLineChanged(this, e);
		}

		/// <summary>
		/// Raises the VScrollBarVisibleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnVScrollBarVisibleChanged(EventArgs e)
		{
			if (VScrollBarVisibleChanged != null)
				VScrollBarVisibleChanged(this, e);
		}

		/// <summary>
		/// Raises the HexCasingChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnHexCasingChanged(EventArgs e)
		{
			if (HexCasingChanged != null)
				HexCasingChanged(this, e);
		}

		/// <summary>
		/// Raises the HorizontalByteCountChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnHorizontalByteCountChanged(EventArgs e)
		{
			if (HorizontalByteCountChanged != null)
				HorizontalByteCountChanged(this, e);
		}

		/// <summary>
		/// Raises the VerticalByteCountChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnVerticalByteCountChanged(EventArgs e)
		{
			if (VerticalByteCountChanged != null)
				VerticalByteCountChanged(this, e);
		}

		/// <summary>
		/// Raises the CurrentLineChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCurrentLineChanged(EventArgs e)
		{
			if (CurrentLineChanged != null)
				CurrentLineChanged(this, e);
		}

		/// <summary>
		/// Raises the CurrentPositionInLineChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCurrentPositionInLineChanged(EventArgs e)
		{
			if (CurrentPositionInLineChanged != null)
				CurrentPositionInLineChanged(this, e);
		}


		/// <summary>
		/// Raises the Copied event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCopied(EventArgs e)
		{
			if (Copied != null)
				Copied(this, e);
		}

		/// <summary>
		/// Raises the CopiedHex event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCopiedHex(EventArgs e)
		{
			if (CopiedHex != null)
				CopiedHex(this, e);
		}

		/// <summary>
		/// Raises the MouseDown event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnMouseDown()", "HexBox");

			if (!Focused)
				Focus();

			if (e.Button == MouseButtons.Left)
				SetCaretPosition(new Point(e.X, e.Y));

			base.OnMouseDown(e);
		}

		/// <summary>
		/// Raises the MouseWhell event
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
            Int32 linesToScroll = -(e.Delta * SystemInformation.MouseWheelScrollLines / 120);
			this.PerformScrollLines(linesToScroll);

			base.OnMouseWheel(e);
		}


		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			UpdateRectanglePositioning();
		}

		/// <summary>
		/// Raises the GotFocus event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnGotFocus()", "HexBox");

			base.OnGotFocus(e);

			CreateCaret();
		}

		/// <summary>
		/// Raises the LostFocus event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnLostFocus()", "HexBox");

			base.OnLostFocus(e);

			DestroyCaret();
		}

		void _byteProvider_LengthChanged(Object sender, EventArgs e)
		{
			UpdateScrollSize();
		}
		#endregion
    }
}
