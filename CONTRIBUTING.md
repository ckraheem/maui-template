# Contributing to MAUI Template

Thank you for considering contributing to this project! This document provides guidelines and instructions for contributing.

## Code of Conduct

By participating in this project, you agree to:
- Be respectful and inclusive
- Provide constructive feedback
- Focus on what is best for the community
- Show empathy towards others

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check existing issues to avoid duplicates.

When creating a bug report, include:
- **Clear title** describing the issue
- **Detailed description** of the problem
- **Steps to reproduce** the behavior
- **Expected behavior** vs actual behavior
- **Screenshots** if applicable
- **Environment details**:
  - OS and version
  - .NET version
  - MAUI version
  - Device/Simulator details

### Suggesting Enhancements

Enhancement suggestions are welcome! Please include:
- **Use case**: Why is this enhancement needed?
- **Detailed description** of the proposed functionality
- **Examples** of how it would work
- **Alternatives considered**

### Pull Requests

1. **Fork the repository** and create your branch from `main`
2. **Make your changes** following our coding standards
3. **Test your changes** thoroughly
4. **Update documentation** if needed
5. **Commit with clear messages** following our guidelines
6. **Submit a pull request**

## Development Setup

### Prerequisites

- .NET 10 SDK
- Visual Studio 2022 (17.12+) or VS Code with C# Dev Kit
- Git
- Platform-specific SDKs (Android, iOS, etc.)

### Getting Started

```bash
# Clone your fork
git clone https://github.com/YOUR_USERNAME/maui-template.git
cd maui-template

# Add upstream remote
git remote add upstream https://github.com/ckraheem/maui-template.git

# Create a branch
git checkout -b feature/your-feature-name

# Make changes and test
dotnet restore
dotnet build
dotnet test
```

## Coding Standards

### C# Guidelines

- Follow [Microsoft's C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable and method names
- Keep methods small and focused
- Add XML documentation for public APIs
- Use async/await for asynchronous operations

### XAML Guidelines

- Use `x:DataType` for compiled bindings
- Keep XAML files clean and readable
- Use proper indentation (4 spaces)
- Group related properties together
- Use resource dictionaries for reusable styles

### Architecture Guidelines

- Follow MVVM pattern strictly
- Use dependency injection for all services
- ViewModels should not reference Views
- Keep business logic in services
- Use interfaces for testability

### Naming Conventions

**C# Files:**
- ViewModels: `*ViewModel.cs`
- Services: `*Service.cs`
- Interfaces: `I*.cs`
- Pages: `*Page.xaml` and `*Page.xaml.cs`

**Namespaces:**
- Follow folder structure
- Use PascalCase
- Example: `MauiTemplate.ViewModels`

**Methods:**
- Use PascalCase
- Async methods end with `Async`
- Example: `LoadDataAsync()`

**Properties:**
- Use PascalCase for public
- Use _camelCase for private fields
- Example: `public string Title { get; set; }`

## Commit Messages

Follow [Conventional Commits](https://www.conventionalcommits.org/):

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation only
- `style`: Code style changes (formatting)
- `refactor`: Code refactoring
- `test`: Adding tests
- `chore`: Maintenance tasks

**Examples:**
```
feat(auth): add Azure AD authentication support

Implements Azure AD authentication using MSAL library.
Adds configuration options in appsettings.json.

Closes #123
```

```
fix(navigation): resolve deep linking crash on Android

Deep links were causing app crash on Android 12+.
Added proper intent filter handling.

Fixes #456
```

## Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "FullyQualifiedName~MyTest"

# Generate coverage report
dotnet test --collect:"XPlat Code Coverage"
```

### Writing Tests

- Write unit tests for ViewModels
- Write integration tests for services
- Mock dependencies using interfaces
- Aim for high code coverage (>80%)
- Test edge cases and error conditions

Example:
```csharp
[Fact]
public async Task LoginAsync_WithValidCredentials_ReturnsTrue()
{
    // Arrange
    var mockTokenStorage = new Mock<ITokenStorageService>();
    var service = new AuthenticationService(mockTokenStorage.Object);

    // Act
    var result = await service.LoginAsync("user", "pass");

    // Assert
    Assert.True(result);
    mockTokenStorage.Verify(x => x.SaveTokenAsync(It.IsAny<AuthToken>()), Times.Once);
}
```

## Documentation

### Code Documentation

- Add XML documentation for public APIs
- Include `<summary>`, `<param>`, and `<returns>` tags
- Explain complex logic with comments
- Keep documentation up to date

Example:
```csharp
/// <summary>
/// Authenticates a user with username and password.
/// </summary>
/// <param name="username">The user's username</param>
/// <param name="password">The user's password</param>
/// <param name="cancellationToken">Cancellation token</param>
/// <returns>True if authentication successful, false otherwise</returns>
public async Task<bool> LoginAsync(
    string username, 
    string password, 
    CancellationToken cancellationToken = default)
{
    // Implementation
}
```

### README Updates

- Update README.md for new features
- Add examples for new functionality
- Keep feature list current
- Update setup instructions if changed

## Pull Request Process

1. **Update documentation** for any public API changes
2. **Add tests** for new functionality
3. **Ensure all tests pass** locally
4. **Update CHANGELOG.md** with your changes
5. **Request review** from maintainers
6. **Address feedback** promptly
7. **Squash commits** before merge (if requested)

### PR Checklist

- [ ] Code follows project style guidelines
- [ ] Self-review of code completed
- [ ] Comments added for complex logic
- [ ] Documentation updated
- [ ] Tests added/updated
- [ ] All tests passing
- [ ] No new warnings introduced
- [ ] Changes don't break existing functionality

## Review Process

### For Contributors

- Be patient - reviews may take time
- Be responsive to feedback
- Be open to suggestions
- Provide context for decisions

### For Reviewers

- Be constructive and respectful
- Focus on the code, not the person
- Explain reasoning for suggestions
- Approve when requirements are met

## Release Process

1. Version bump in project file
2. Update CHANGELOG.md
3. Create release tag
4. Generate release notes
5. Publish to NuGet (if applicable)

## Questions?

- Check existing documentation
- Search existing issues
- Ask in GitHub Discussions
- Contact maintainers

## Recognition

Contributors will be:
- Listed in CONTRIBUTORS.md
- Mentioned in release notes
- Credited in commits

Thank you for contributing! 🎉
