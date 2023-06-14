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

    public enum type 
    {
        StarterClass,
        SynergizedClass,
        HyperClass
    }

    public StarterClass starterClass;
    public SynergizedClass synergizedClass;
    public HyperClass hyperClass;
    public type _type;

    public Class(StarterClass starterClass, type type)
    {
        this.starterClass = starterClass;
        this._type = type;
    }

    public Class(SynergizedClass synergizedClass, type type)
    {
        this.synergizedClass = synergizedClass;
        this._type = type;
    }

    public Class(HyperClass hyperClass, type type)
    {
        this.hyperClass = hyperClass;
        this._type = type;
    }
}