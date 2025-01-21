﻿using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ApproveMe.Api.Commons;

public class RoutePrefixConvention : IApplicationModelConvention
{
    private readonly AttributeRouteModel _routePrefix;

    public RoutePrefixConvention(IRouteTemplateProvider route)
    {
        _routePrefix = new AttributeRouteModel(route);
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var selector in application.Controllers.SelectMany(c => c.Selectors))
        {
            var attributeModel = selector.AttributeRouteModel != null ? 
                AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel) : 
                _routePrefix;

            selector.AttributeRouteModel = attributeModel;
        }
    }
}