using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{

    [SerializeField]
    public Light sunLight;
    [SerializeField]
    public Light moonLight;
    //Struc de calendario que contiene el numero de le fecha dia, mes, año 
    //A su vez hay una struct anidada de dia que forma la hora minuto y segundo del calendario
    public struct Calendar
    {
        public struct Day
        {
            public int hour;
            public int minute;
            public int second;
            Light sunLight;
            Light moonLight;

            //Calcula la hora del dia a partir del dia normalizado entre 0 y 1
            public void setDayTime(float time)
            {
                float seconds_of_day = 86400 * time;
                float minutes = seconds_of_day / 60;
                second = (int)(seconds_of_day % 60);
                hour   = (int)(minutes / 60);
                minute = (int)(minutes % 60);
            }
            public void setSunAndMoon(Light sun, Light moon)
            {
                sunLight = sun;
                moonLight = moon;
            }
            public void setSunRotation(float time)
            {
                sunLight.transform.rotation = Quaternion.Euler(270 + 360 * time, 0, 0);
                moonLight.transform.rotation = Quaternion.Euler(90 +  360 * time, 0, 0);
            }
        }

        public Day day;

        public int number_of_day;
        public int month;
        public int year;
        public int season;
        public float day_duration;
        public double world_moment;


        //Establece la duracion del dia en el calendario
        public void setDayDuration(int seconds) { day_duration = seconds; }

        //Actualiza el calendario a partir del time acumulado del mundo
        public void update(double time)
        {
            world_moment = time;
            int days;
            int months;
            days = (int)(time / day_duration);
            day.setDayTime((float)((time % day_duration)/day_duration));
            months = days / 30;
            number_of_day = days % 30+1;
            year = months / 12;
            month = months % 12+1;
            day.setSunRotation((float)((time % day_duration) / day_duration));
            //print(number_of_day+"/"+month+"/"+year + "  Time:" + day.hour + ":" + day.minute + ":" +day.second);
            //print(season);

            season = month / 4;
        }

        public static double operator -(Calendar calendar , Calendar calendar_minus)
        {
            return calendar.world_moment - calendar_minus.world_moment;
        }

        public int get_Days(double time)
        {
            return (int)(time / day_duration)+1;
        }
    }

    //Valor de la duracion del dia en segundos para modificarlo desde el editor
    [SerializeField] int day_duration;

    //Tiempo acumulado del mundo
    public double world_time;
    
    //Calendario del mundo
    public Calendar world_calendar;

    // Start is called before the first frame update
    private static TimeController instance = null;

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Game Instance Singleton
    public static TimeController Instance
    {
        get
        {
            return instance;
        }
    }

    void Start()
    {
        world_calendar.setDayDuration(day_duration);
        world_calendar.day.setSunAndMoon(sunLight,moonLight);
    }

    // Update is called once per frame
    void Update()
    {
        world_time += Time.deltaTime;
        world_calendar.update(world_time);
        
    }
}
