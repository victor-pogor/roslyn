' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.



Namespace Microsoft.CodeAnalysis.Editor.UnitTests.GoToDefinition
    <UseExportProvider>
    <Trait(Traits.Feature, Traits.Features.GoToDefinition)>
    Public Class CSharpGoToTypeDefinitionTests
        Inherits GoToTypeDefinitionTestsBase

        ' TODO add metadata parameter to TestAsync
        Private Overloads Shared Async Function TestAsync(source As String, Optional expectedResult As Boolean = True) As Task
            Await TestAsync(source, LanguageNames.CSharp, expectedResult)
        End Function

#Region "Classes"
        <WpfFact, WorkItem("https://github.com/dotnet/roslyn/issues/2505")>
        Public Async Function TestCSharpGoToTypeDefinitionClass() As Task
            Dim source =
                "class [|CustomType|] { }

                class C
                {
                    public CustomType P { get; set; }
                }

                class D : C
                {
                    void M()
                    {
                        var x = $$P;
                    }
                }"

            Await TestAsync(source)
        End Function

        <WpfFact, WorkItem("https://github.com/dotnet/roslyn/issues/2505")>
        Public Async Function TestCSharpGoToTypeDefinitionMetadataClass() As Task
            Dim source =
                "class C
                {
                    public string P { get; set; }
                }

                class D : C
                {
                    void M()
                    {
                        var x = $$P;
                    }
                }"

            Await TestAsync(source)
        End Function

        <WpfFact, WorkItem("https://github.com/dotnet/roslyn/issues/2505")>
        Public Async Function TestCSharpGoToTypeDefinitionSameClass() As Task
            Dim source =
                "class [|C|]
                {
                    public C P { get; set; }
                }

                class D
                {
                    void M()
                    {
                        var x = new C().$$P;
                    }
                }"

            Await TestAsync(source)
        End Function

        <WpfFact, WorkItem("https://github.com/dotnet/roslyn/issues/2505")>
        Public Async Function TestCSharpGoToTypeDefinitionNestedClass() As Task
            Dim source =
                "class Outer
                {
                    class [|Inner|]
                    {
                    }

                    Inner property { get; set; }
                    Inner someObj = pro$$perty;
                }"

            Await TestAsync(source)
        End Function

        <WpfFact, WorkItem("https://github.com/dotnet/roslyn/issues/2505")>
        Public Async Function TestCSharpGoToTypeDefinitionClassDifferentFiles() As Task
            Dim workspace =
                <Workspace>
                    <Project Language="C#" CommonReferences="true">
                        <Document FilePath="CustomType.cs">
                            class [|CustomType|] { }
                        </Document>
                        <Document FilePath="D.cs">
                            class D
                            {
                                CustomType customType;
                                void M()
                                {
                                    var x = $$customType;
                                }
                            }
                        </Document>
                    </Project>
                </Workspace>

            Await TestAsync(workspace)
        End Function

        <WpfFact, WorkItem("https://github.com/dotnet/roslyn/issues/2505")>
        Public Async Function TestCSharpGoToTypeDefinitionPartialClass() As Task
            Dim workspace =
                <Workspace>
                    <Project Language="C#" CommonReferences="true">
                        <Document FilePath="CustomType.cs">
                            partial class nothing { }
                        </Document>
                        <Document FilePath="CustomType.Partial1.cs">
                            partial class [|CustomType|] { }
                        </Document>
                        <Document FilePath="CustomType.Partial2.cs">
                            partial class [|CustomType|] { }
                        </Document>
                        <Document FilePath="D.cs">
                            class D
                            {
                                CustomType customType;
                                void M()
                                {
                                    var x = custom$$Type;
                                }
                            }
                        </Document>
                    </Project>
                </Workspace>

            Await TestAsync(workspace)
        End Function

#End Region

    End Class
End Namespace
