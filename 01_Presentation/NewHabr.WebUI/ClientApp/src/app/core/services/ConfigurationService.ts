import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Configuration } from "../models/Configuration";


@Injectable({
    providedIn: 'root',
})
export class ConfigurationService {

    _configuration: Configuration;
    public get configuration(): Configuration {
        return this._configuration;
    }
    public set configuration(value: Configuration) {
        this._configuration = value;
    }

    constructor(private http: HttpClient) { }

    getConfigAsync(): Observable<Configuration> {
        return this.http.get<Configuration>('assets/appconfig.json');
    }
}
