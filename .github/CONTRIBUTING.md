# How to contribute

First of all, thank you for wanting to contribute to Nancy! We really appreciate all the awesome support we get from our community. We want to keep it as easy as possible to contribute changes that get things working in your environment. There are a few guidelines that we need contributors to follow so that we have a chance of keeping on top of things.

- [Making Changes](#making-changes)
  - [Handling Updates from Upstream/Master](#handling-updates-from-upstreammaster)
  - [Sending a Pull Request](#sending-a-pull-request)
- [Style Guidelines](#style-guidelines)

## Making Changes

1. [Fork](http://help.github.com/forking/) on GitHub
1. Clone your fork locally
1. Configure the upstream repo (`git remote add upstream git://github.com/NancyFx/Nancy.Bootstrappers.Autofac`)
1. Create a local branch (`git checkout -b myBranch`)
1. Work on your feature
1. Rebase if required (see below)
1. Push the branch up to GitHub (`git push origin myBranch`)
1. Send a Pull Request on GitHub

You should **never** work on a clone of master, and you should **never** send a pull request from master - always from a branch. The reasons for this are detailed below.

### Handling Updates from Upstream/Master

While you're working away in your branch it's quite possible that your upstream master (most likely the canonical NancyFx version) may be updated. If this happens you should:

1. [Stash](http://git-scm.com/book/en/Git-Tools-Stashing) any un-committed changes you need to
1. `git checkout master`
1. `git pull upstream master`
1. `git checkout myBranch`
1. `git rebase master myBranch`
1. `git push origin master` - (optional) this makes sure your remote master is up to date

This ensures that your history is "clean" i.e. you have one branch off from master followed by your changes in a straight line. Failing to do this ends up with several "messy" merges in your history, which we don't want. This is the reason why you should always work in a branch and you should never be working in, or sending pull requests from, master.

If you're working on a long running feature then you may want to do this quite often, rather than run the risk of potential merge issues further down the line.

### Sending a Pull Request

While working on your feature you may well create several branches, which is fine, but before you send a pull request you should ensure that you have rebased back to a single "Feature branch". We care about your commits, and we care about your feature branch; but we don't care about how many or which branches you created while you were working on it :smile:.

When you're ready to go you should confirm that you are up to date and rebased with upstream/master (see "Handling Updates from Upstream/Master" above), and then:

1. `git push origin myBranch`
1. Send a descriptive [Pull Request](http://help.github.com/pull-requests/) on GitHub - making sure you have selected the correct branch in the GitHub UI!
1. Wait for @TheCodeJunkie to merge your changes in and reformat all of your code because he has StyleCop OCD :wink:.

And remember; **A pull-request with tests is a pull-request that's likely to be pulled in.** :grin: Bonus points if you document your feature in our [wiki](https://github.com/NancyFx/Nancy/wiki) once it has been pulled in

## Style Guidelines

- Indent with 4 spaces, **not** tabs.
- No underscore (`_`) prefix for member names.
- Use `this` when accessing instance members, e.g. `this.Name = "TheCodeJunkie";`.
- Use the `var` keyword unless the inferred type is not obvious.
- Use the C# type aliases for types that have them, e.g. `int` instead of `Int32`, `string` instead of `String` etc.
- Use meaningful names (no hungarian notation).
- Wrap `if`, `else` and `using` blocks (or blocks in general, really) in curly braces, even if it's a single line.
- Put `using` statements inside namespace.
- Pay attention to whitespace and extra blank lines
- Absolutely **no** regions

> If you are a ReSharper user, you can make use of our `.DotSettings` file to ensure you cover as many of our style guidelines as possible. There may be some style guidelines which are not covered by the file, so please pay attention to the style of existing code.
