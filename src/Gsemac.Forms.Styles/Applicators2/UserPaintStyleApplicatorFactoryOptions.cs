﻿namespace Gsemac.Forms.Styles.Applicators2 {

    public class UserPaintStyleApplicatorFactoryOptions :
        IUserPaintStyleApplicatorFactoryOptions {

        public bool CustomScrollBarsEnabled { get; set; } = true;

        public static UserPaintStyleApplicatorFactoryOptions Default => new UserPaintStyleApplicatorFactoryOptions();

    }

}