using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Class
{
    public enum StarterClass
    {
        INSANER,
        WARRIOR,
        MAGICIAN,
        SUMMONER
    }

    public enum SynergizedClass
    {
        PENETRATOR,
        WIZARD,
        ELEMENTOR,
        ALPHA_TRAINER,
        SIGMA_TRAINER,
        BETA_TRAINER
    }

    public enum HyperClass
    {
        ZEUS,
        DIONE,
        ARETMIS,
        AVATAR,
        HEKATE
    }

    public enum classType 
    {
        STARTER,
        SYNERGIZED,
        HYPER
    }

    public StarterClass starterClass;
    public SynergizedClass synergizedClass;
    public HyperClass hyperClass;
    public classType type;

    public Class(StarterClass starterClass, classType type)
    {
        this.starterClass = starterClass;
        this.type = type;
    }

    public Class(SynergizedClass synergizedClass, classType type)
    {
        this.synergizedClass = synergizedClass;
        this.type = type;
    }

    public Class(HyperClass hyperClass, classType type)
    {
        this.hyperClass = hyperClass;
        this.type = type;
    }
}