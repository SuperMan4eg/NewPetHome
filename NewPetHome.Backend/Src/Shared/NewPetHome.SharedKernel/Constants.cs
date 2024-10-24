namespace NewPetHome.SharedKernel;

public class Constants
{
    public const int MAX_LOW_TEXT_LENGTH = 100;
    public const int MAX_HIGH_TEXT_LENGTH = 2000;
    public const int MAX_PHONE_NUMBER_LENGTH = 10;

    public const string DATABASE = "Database";
    public const string BUCKET_NAME = "photos";

    public static readonly string[] PERMITTED_PET_STATUSES_FROM_VOLUNTEER = ["LookingHome", "InTreatment"];
}