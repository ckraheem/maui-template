# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-01-14

### Added
- Initial release of MAUI Template
- MVVM architecture with CommunityToolkit.Mvvm
- Shell Flyout navigation with custom header/footer
- Multi-provider authentication (JWT, Azure AD, Auth0)
- Secure token storage with automatic refresh
- SQLite offline cache with generic repository pattern
- HttpClient API service with typed endpoints
- ViewModel-based navigation service
- Global loading indicator service
- Serilog logging with file output
- Error boundary for unhandled exceptions
- Deep linking support for Android/iOS
- Login page with multiple authentication options
- Items list page with pull-to-refresh
- Item detail page with CRUD operations
- Settings modal example
- Role-based UI visibility converters
- Performance helpers (throttle, debounce, measure)
- Connectivity helper for network awareness
- Custom behaviors (EventToCommand)
- Lifecycle hooks for app state management
- x:DataType compiled bindings throughout
- Async/await with cancellation token support
- Comprehensive documentation (README, ARCHITECTURE, QUICKSTART, CONTRIBUTING)
- Configuration file example (appsettings.json)
- .gitignore for build artifacts

### Developer Experience
- Full dependency injection setup
- BaseViewModel with common functionality
- Generic database service for any entity
- Automatic cache sync when online
- Proper error handling at all levels
- Structured logging with context
- Fast startup optimization
- Low memory footprint design

### Platforms Supported
- Android (net10.0-android)
- iOS (net10.0-ios) - when on macOS
- Windows (net10.0-windows) - when on Windows
- macOS (net10.0-maccatalyst) - when on macOS

## [Unreleased]

### Planned Features
- Biometric authentication
- Push notifications support
- Background sync service
- Unit test examples
- UI test examples
- CI/CD pipeline templates
- Docker support for API
- GraphQL client option
- SignalR real-time support

---

## Version History

### How to Read This Changelog

- **Added**: New features
- **Changed**: Changes in existing functionality
- **Deprecated**: Soon-to-be removed features
- **Removed**: Removed features
- **Fixed**: Bug fixes
- **Security**: Security vulnerability fixes

### Links

- [GitHub Releases](https://github.com/ckraheem/maui-template/releases)
- [Issue Tracker](https://github.com/ckraheem/maui-template/issues)
