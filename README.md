# Gsemac.Forms.Styles
WinForms theming library using CSS.

## About

This library contains classes for theming WinForms applications using cascading style sheets (CSS). CSS makes it easy to change themes at runtime and allow users to create their own custom themes in a familiar way. 

One novel characteristic of this library is its ability to "skin" native controls, so you can easily theme an application built with native controls without having to switch over to any new controls.

## How to use

Once a style sheet has been loaded (using the `StyleSheet` class), a "style applicator" can be used to apply the style to controls. There are two style applicators: `UserPaintStyleApplicator`, and `PropertyStyleApplicator`. The former allows for complex drawing, while the latter attempts to approximate the style using the control's public properties.

```csharp
IStyleSheet styleSheet = StyleSheet.FromFile("DarkUI.css");
IStyleApplicator applicator = new UserPaintStyleApplicator(styleSheet);

applicator.ApplyStyles(form);
```

An example project `TestForm` is included for demonstration. It has an example style sheet, `DarkUI.css`, which was designed after [DarkUI](https://github.com/RobinPerris/DarkUI).

<p align="center">
  <img src="https://user-images.githubusercontent.com/28276798/86505924-776ba780-bd87-11ea-9fd2-5ab0363a0ea9.png" />
</p>

## Notes

Only a limited subset of CSS properties and selectors have been implemented at this point. Implemented properties include `BackgroundColor`, `BackgroundImage`, `Border`, `BorderColor`, `BorderRadius`, `BorderStyle`, `BorderWidth`, `Color`, and `Opacity`.

Supported controls and components include `Button`, `CheckBox`, `ComboBox`, `DataGridView`, `Label`, `ListBox`, `ListView`, `NumericUpDown`, `PictureBox`, `ProgressBar`, `RadioButton`, `RichTextBox`, `TabControl`, `TextBox`, `ToolStrip`, `ToolTip`, and `TreeView`.

This project is currently very much a WIP, so some things might not be working perfectly just yet.
