using System.Collections;
using UnityEngine;

public class SectionSpawner : MonoBehaviour
{
    [SerializeField] [Tooltip("The prefabs for the different section types")] private GameObject[] sectionTypes;
    [SerializeField] [Tooltip("The SectionMover GameObject")] private SectionMover sectionMover;
    [SerializeField] [Tooltip("The Y position at which new sections will spawn from")] private float topSpawningPoint;
    [SerializeField] [Tooltip("The Y position at which sections will be destroyed")] private float bottomDestructionPoint;
    private ArrayList activeSections;

    private void Start()
    {
        activeSections = new ArrayList();

        // Continually spawning new sections on top of eachother until topSpawningPoint is reached
        bool topSpawningPointHit = false;
        while (!topSpawningPointHit)
        {
            GameObject newSection = selectRandomSectionType();

            Vector3 newSectionPosition;
            if (activeSections.Count == 0)
            {
                newSectionPosition = new Vector3(0, 0, 0);
            }
            else
            {
                GameObject previousSection = (GameObject)activeSections[0];
                Bounds previousSectionBounds = previousSection.GetComponent<Renderer>().bounds;
                Bounds newSectionBounds = newSection.GetComponent<Renderer>().bounds;
                // The Y value of the new section position is the previous section's position, plus the height of the top half of the previous section, plus the bottom half of the new section
                newSectionPosition = new Vector3(0, previousSection.transform.position.y + (previousSectionBounds.max.y - previousSectionBounds.center.y) + (newSectionBounds.center.y - newSectionBounds.min.y), 0);
                float sectionYTop = newSectionPosition.y + (newSectionBounds.center.y - newSectionBounds.min.y);
                if (sectionYTop > topSpawningPoint)
                {
                    topSpawningPointHit = true;
                }
            }

            activeSections.Insert(0, Instantiate(newSection, newSectionPosition, new Quaternion(0, 0, 0, 0)));
        }

        sectionMover.sections = activeSections;
	}

    // Randomly selects a valid section type
    private GameObject selectRandomSectionType()
    {
        // If there's only one section type, skip the random section type selection process and just use the only existing section type
        if (sectionTypes.Length == 1)
        {
            return sectionTypes[0];
        }
        else
        {
            /*
             * If no section types have already been randomly selected, any index postion from the sectionTypes array can be the one that's randomly selected.
             * Otherwise, any sectionType index position excluding its last index position can be the one that's randomly selected.
             * After the first randomly selected section type, each time a section type is randomly selected it will be swapped with the section type at the end of the sectionTypes array.
             * This makes randomly selecting a section type that wasn't the previously spawned section type simple to manage.
             * Knowing that the last section type in the selectionTypes array will be the previously used one,
             * we can generate a random index position in the range of 0 to the length of the selectionTypes array minus one,
             * therefore excluding the index position of the previously used section type from being selected.
            */

            int randomSectiontypeIndex;
            if (activeSections.Count == 0)
            {
                randomSectiontypeIndex = RandomNumber.Generate(0, sectionTypes.Length);
            }
            else
            {
                randomSectiontypeIndex = RandomNumber.Generate(0, sectionTypes.Length - 1);
            }

            GameObject newSection = sectionTypes[randomSectiontypeIndex];
            if (randomSectiontypeIndex != sectionTypes.Length - 1)
            {
                sectionTypes[randomSectiontypeIndex] = sectionTypes[sectionTypes.Length - 1];
                sectionTypes[sectionTypes.Length - 1] = newSection;
            }
            return newSection;
        }
    }

	private void FixedUpdate()
    {
        // Destroying the current lowest section when it's fully underneath bottomDestructionPoint
        GameObject lastSection = (GameObject)activeSections[activeSections.Count - 1];
        Bounds lastSectionBounds = lastSection.GetComponent<Renderer>().bounds;
        float lastSectionYTop = lastSection.transform.position.y + (lastSectionBounds.center.y - lastSectionBounds.min.y);
        if (lastSectionYTop <= bottomDestructionPoint)
        {
            activeSections.RemoveAt(activeSections.Count - 1);
            sectionMover.sections = activeSections;
            Destroy(lastSection);
        }

        // Spawning a new section when the current highest section is fully below topSpawningPoint
        GameObject firstSection = (GameObject)activeSections[0];
        Bounds firstSectionBounds = firstSection.GetComponent<Renderer>().bounds;
        float firstSectionYTop = firstSection.transform.position.y + (firstSectionBounds.center.y - firstSectionBounds.min.y);
        if (firstSectionYTop <= topSpawningPoint)
        {
            GameObject newSection;
            if (sectionTypes.Length == 1)
            {
                newSection = sectionTypes[0];
            }
            else
            {
                int randomSectiontypeIndex = RandomNumber.Generate(0, sectionTypes.Length - 1);
                newSection = sectionTypes[randomSectiontypeIndex];
                sectionTypes[randomSectiontypeIndex] = sectionTypes[sectionTypes.Length - 1];
                sectionTypes[sectionTypes.Length - 1] = newSection;
            }
            Bounds newSectionBounds = newSection.GetComponent<Renderer>().bounds;
            Vector3 newSectionPosition = new Vector3(0, firstSectionYTop + (newSectionBounds.center.y - newSectionBounds.min.y), 0);
            activeSections.Insert(0, Instantiate(newSection, newSectionPosition, new Quaternion(0, 0, 0, 0)));
            sectionMover.sections = activeSections;
        }
    }
}