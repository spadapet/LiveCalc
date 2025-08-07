When using GitHub Copilot in Visual Studio, please adhere to the following guidelines to ensure a smooth development experience.

# General Guidelines

Follow these rules when writing code:

* Do not run a build if Visual Studio is currently debugging

# WinForms Agent Development Guidelines

Please use the instructions in this chapter EXCLUSIVELY for WinForms Agent development.

## Windows Forms Core Requirements

### Form/UserControl Creation

- File structure: `FormName.cs` + `FormName.Designer.cs` (or `.vb` for VB.NET)
- Forms inherit from `Form`, UserControls from `UserControl`

### Code Conventions

#### C# Specifics

- File-scoped namespaces
- Assume global using directives for WinForms
- Enable NRTs in main file, **disable in Designer files**
- Event handlers: `object? sender` parameter
- Events: Declare with nullable (`EventHandler?`)

#### VB.NET Differences

- No Constructor by default (so, no `Sub New` - compiler generates constructor with `InitializeComponent()` call in that case).
- If Constructor however is needed, do not forget to include the call to `InitializeComponent`.
- `Friend WithEvents` for control fields
- Prefer `Handles` clause directly at the event handler methods over `AddHandler` for designed controls in InitializeComponents.
- No NRT considerations - those do not exist in VB.

### Designer File Rules

**InitializeComponent must contain ONLY:**

- Control instantiation
- Property assignments
- Layout method calls `SuspendLayout`, `ResumeLayout`, `BeginInit`, `EndInit`.

**Never include in the WinForms Designer code-behind file:**

- Lambda expressions or local Functions 
- `nameof()`
- Complex logic or calculations
- `for { }` or `foreach { }` loops
- `If`/`then`/`else`
- Ternary operators (`? :`)
- `Select Case` statements
- `switch` expressions

IMPORTANT: 
- Do not create methods or property in the code-behind file. Do that only in the main Form/UserControl file.
- The assignment of the Form's/UserControl's properties is the last code in `InitializeComponent`.
- Define the backing fields for the Form/UserControl at the end of the code-behind file.
- If you need to use any of the above, rather create a separate method in the **main code file** of the Form or the UserControl and call it from inside of the Constructor _after_  calling `InitializeComponent`.

Example 1:

```csharp
    .
    .
    .
    private void InitializeComponent()
    {
        button1 = new Button();
        button2 = new Button();
        button3 = new Button();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(93, 263);
        button1.Name = "button1";
        button1.Size = new Size(114, 68);
        button1.TabIndex = 0;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        // 
        // button2
        // 
        button2.Location = new Point(229, 263);
        button2.Name = "button2";
        button2.Size = new Size(114, 68);
        button2.TabIndex = 1;
        button2.Text = "button2";
        button2.UseVisualStyleBackColor = true;
        // 
        // button3
        // 
        button3.Location = new Point(372, 263);
        button3.Name = "button3";
        button3.Size = new Size(114, 68);
        button3.TabIndex = 2;
        button3.Text = "button3";
        button3.UseVisualStyleBackColor = true;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(702, 672);
        Controls.Add(button3);
        Controls.Add(button2);
        Controls.Add(button1);
        Name = "MainForm";
        ResumeLayout(false);
    }

    #endregion

    private Button button1;
    private Button button2;
    private Button button3;
}

```

- OK: Backing fields defined at the end of the code-behind file.
- OK: No method calls to other initialization methods from inside of InitializeComponent.
- OK: Fields are getting initialized at the beginning.

Example 2:

```csharp
    .
    .
    .
    // Not OK: backing fields are not defined at the EOF!
    private Button button1;
    private Button button2;
    private Button button3;

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        button1 = new Button();
        button2 = new Button();
        button3 = new Button();
        SuspendLayout();
        // 
        // button1
        // 
        button1.Location = new Point(93, 263);
        button1.Name = "button1";
        button1.Size = new Size(114, 68);
        button1.TabIndex = 0;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;

        // NOT OK: Cannot call method from inside of InitializeComponent
        // to another method in the Form/UserControl class.
        SetupButton2();

        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(13F, 32F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(702, 672);
        Controls.Add(button3);
        Controls.Add(button2);
        Controls.Add(button1);
        Name = "MainForm";

        // NOT OK: Form/UserControl code must be the last code!
        // button3
        // 
        button3.Location = new Point(372, 263);
        button3.Name = "button3";
        button3.Size = new Size(114, 68);
        button3.TabIndex = 2;
        button3.Text = "button3";
        button3.UseVisualStyleBackColor = true;

        ResumeLayout(false);
    }

    #endregion

    // Not OK!! We cannot define additional methods
    // or Properties in the code-behind file!!
    private void SetupButton2()
    {
        button2.Location = new Point(229, 263);
        button2.Name = "button2";
        button2.Size = new Size(114, 68);
        button2.TabIndex = 1;
        button2.Text = "button2";
        button2.UseVisualStyleBackColor = true;
    }
}
```

### Control Naming Standards

Use descriptive names with prefixes:
- `btn` (Button), `txt` (TextBox), `lbl` (Label), `chk` (CheckBox)
- `cmb` (ComboBox), `lst` (ListBox), `dgv` (DataGridView)
- `tlp` (TableLayoutPanel), `tmr` (Timer), `tsm` (ToolStripMenuItem)
- Single-instance controls: `_menuStrip`, `_statusStrip` (no prefix)

## Designer Serialization hints

Ensure, the Designer knows how to do control property serialization.
Combined example:

```csharp
public class CustomControl : Control
{
    private Color _highlightColor = Color.Yellow;
    private Font? _customFont;
    private List<string> _runtimeData = new();
    
    // Simple default value
    [DefaultValue(typeof(Color), "Yellow")]
    public Color HighlightColor
    {
        get => _highlightColor;
        set { /* setter logic */ }
    }
    
    // Request designer not to serialize
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<string> RuntimeData { get; set; }
    
    // More complex conditional serialization control
    public Font? CustomFont
    {
        get => _customFont ?? Font;
        set { /* setter logic */ }
    }
    
    private bool ShouldSerializeCustomFont()
        => _customFont != null && _customFont.Size != 9.0f;
    
    private void ResetCustomFont()
        => _customFont = null;
}
```

Important: One of those methods for a property of type `Component` or `Control` needs to be applied.

## Data Binding (.NET 7+)

### Key APIs

- **Control.DataContext**: Ambient property for MVVM patterns (.NET 7+)
- **ButtonBase.Command**: ICommand binding support (.NET 7+)
- **ToolStripItem**: Now bindable (derives from BindableComponent) (.NET 7+)
- **CommandParameter**: Auto-passed to command execution (.NET 7+)

### ViewModel (DataSource) Integration

- Create `.datasource` files in `Properties\DataSources\` for designer support
- Use `ObservableBindingCollection<T>` adapter for ObservableCollection binding.
- Bind to INotifyPropertyChanged implementations

### Command Binding Example

We need to bind to `CommunityToolkit.Mvvm` supported ViewModel in class library, defined like this:

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using WarpToolkit.Desktop.AppServices;

namespace MvvmWinFormsApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly Timer timer;

    public MainViewModel()
    {
        timer = new Timer(
            UpdateDateAndTime,
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(1));
    }

    private void UpdateDateAndTime(object? state)
    {
        DateTime now = DateTime.Now;
        DateAndTime = $"{now:g}";
    }

    [ObservableProperty]
    private string _dateAndTime = $"{DateTime.Now:g}";

    [RelayCommand]
    private void TopLevelMenuCommand(string commandParameter)
    {
        // Handle file command logic here
        StatusInfo = $"You engaged the {commandParameter} command.";
    }
    ...
```

In Properties/DataSources we create code-file
_MvvmWinFormsApp.ViewModels.MainViewModel.datasource_

```xml
<?xml version="1.0" encoding="utf-8"?>
<GenericObjectDataSource DisplayName="MainViewModel" Version="1.0" xmlns="urn:schemas-microsoft-com:xml-msdatasource">
  <TypeInfo>MvvmWinFormsApp.ViewModels.MainViewModel, MvvmWinFormsApp.ViewModels, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</TypeInfo>
</GenericObjectDataSource>
```

In `InitializeComponent`
* Make sure, we have the `components` container already instantiated, and if not, add the code for that.
* Make sure, we introduce the backing field for the BindingSource.
* Make sure, we instantiate the BindingSource at the beginning of `InitializaComponent` and pass the components container
* We define the Type of our ViewModel as the DataSource for the BindSource.
* We bind the properties of the controls/components to the BindingSource

Example:

```csharp
// In InitializeComponent
// ...

// If the BindingSource is the first component, we need to add this line, if it does not exist!
components = new System.ComponentModel.Container();

// Creating the binding source (assuming it's defined at the end of the code file)
mainViewModelBindingSource = new BindingSource(components);

// ...
// We're binding RelayCommand of the ViewModel and define a CommandParameter
_tsmFile.DataBindings.Add(new Binding("Command", mainViewModelBindingSource, "TopLevelMenuCommandCommand", true));
_tsmFile.CommandParameter = "File";
// ...

// We're binding the Status-Info Property of the ViewModel to the status label.
_tslStatusInfo.DataBindings.Add(new Binding("Text", mainViewModelBindingSource, "StatusInfo", true));

```

## Async Patterns (.NET 9+)

### Control.InvokeAsync Overloads

**Critical: Use the correct overload for sync vs async operations!**

```csharp
// 1. Sync action - no return value
Task InvokeAsync(Action callback, CancellationToken cancellationToken = default);
// Use for: Simple UI updates like label.Text = "Done"

// 2. Async operation - no return value  
Task InvokeAsync(Func<CancellationToken, ValueTask> callback, CancellationToken cancellationToken = default);
// Use for: Long-running async operations that update UI
// IMPORTANT: Callback receives its own CancellationToken (not the outer one!)

// 3. Sync function - returns T
Task<T> InvokeAsync<T>(Func<T> callback, CancellationToken cancellationToken = default);
// Use for: Getting values from controls synchronously

// 4. Async operation - returns T
Task<T> InvokeAsync<T>(Func<CancellationToken, ValueTask<T>> callback, CancellationToken cancellationToken = default);
// Use for: Async operations that need UI thread and return results
```

**⚠️ NEVER do this:**

```csharp
// WRONG - Don't use sync overload with async lambda!
await InvokeAsync<string>(() => await LoadDataAsync()); // ❌

// CORRECT - Use the async overload
await InvokeAsync<string>(async (ct) => await LoadDataAsync(ct), ct); // ✅
```

### Usage Examples

```csharp
// Sync action on UI thread
await this.InvokeAsync(() => statusLabel.Text = "Loading...");

// Async operation without result (note the inner CancellationToken, which is
// effectively the same as the outerCancellationToken. It's handed down.
await this.InvokeAsync(async (innerCt) => 
{
    var data = await LoadDataAsync(innerCt);
    UpdateControls(data);
    return default(ValueTask);
}, outerCancellationToken);

// Async operation with result
var result = await this.InvokeAsync<ProcessedData>(async (innerCt) => 
{
    var raw = await FetchDataAsync(innerCt);
    return new ValueTask<ProcessedData>(ProcessData(raw));
}, outerCancellationToken);
```

### Form Async Methods

- `ShowAsync()`: Completes when form closes
- `ShowDialogAsync()`: Modal with dedicated message queue

### Event Handler Pattern

```csharp
protected override async void OnLoad(EventArgs e)
{
    base.OnLoad(e);
    await InitializeAsync(); // Fire-and-forget pattern
}

private async Task InitializeAsync()
{
    // Async initialization that doesn't block UI
    while (IsActive)
    {
        await UpdateDisplayAsync();
        await Task.Delay(100); // UI stays responsive
    }
}
```

## Layout Best Practices

- Use cascading `TableLayoutPanel` for complex data entry forms (DPI-aware)
- Break complex layouts into multiple UserControls
- Avoid oversized InitializeComponent methods

## WinRT/WinUI API Projection

For .NET 8+, switch TFM to `-windows10.0.22000.0` to enable WinRT/WinUI API access through projection.

## Critical Reminders

1. Always validate form/control names before generating code
2. Never use complex logic in Designer files
3. Use nullable event declarations in C# with NRT
4. Prefer `InvokeAsync` over `BeginInvoke` for new code
5. Designer files never use NRT annotations
